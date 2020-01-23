using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.Models.Common;
using TutorBits.Project;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        protected readonly DBDataAccessService dbDataAccessService_;
        protected readonly ProjectService projectService_;
        protected readonly LambdaAccessService lambdaAccessService_;

        public HealthController(IConfiguration configuration,
                                DBDataAccessService dbDataAccessService,
                                ProjectService projectService,
                                LambdaAccessService lambdaAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            projectService_ = projectService;
            lambdaAccessService_ = lambdaAccessService;
        }

        [HttpGet]
        public async Task<IActionResult> Check()
        {
            if (!(await TestDB()))
            {
                Console.WriteLine("Failed DB Test");
                return BadRequest();
            }

            if (!(await TestFileSystem()))
            {
                Console.WriteLine("Failed FileSystem Test");
                return BadRequest();
            }

            if (!(await TestLambda()))
            {
                Console.WriteLine("Failed Lambda Test");
                return BadRequest();
            }

            return Ok();
        }

        private async Task<bool> TestDB()
        {
            var tutorial = await dbDataAccessService_.CreateBaseModel<Tutorial>(new Tutorial()
            {
                Title = "HEalthCheckTest"
            });

            var lookedUpTutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(null, tutorial.Id);
            if (tutorial.Title != lookedUpTutorial.Title || lookedUpTutorial.Title != "HEalthCheckTest")
            {
                Console.WriteLine("Invalid title on tutorial");
                return false;
            }

            lookedUpTutorial.Description = "HealthCheckTesting";
            await dbDataAccessService_.UpdateBaseModel(lookedUpTutorial);

            lookedUpTutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(null, tutorial.Id);
            if (lookedUpTutorial.Description != "HealthCheckTesting")
            {
                Console.WriteLine("Invalid description on tutorial");
                return false;
            }

            await dbDataAccessService_.DeleteBaseModel<Tutorial>(lookedUpTutorial, true);

            return true;
        }

        private async Task<bool> TestFileSystem()
        {
            var projectId = Guid.NewGuid();
            while (true)
            {
                try
                {
                    if (!(await projectService_.DoesProjectExist(projectId)))
                    {
                        break;
                    }
                    projectId = Guid.NewGuid();
                }
                catch (Exception)
                {
                    break;
                }
            }

            await projectService_.CreateTraceProject(new Tracer.TraceProject()
            {
                Id = projectId.ToString(),
                Duration = 2,
                PartitionSize = 30
            });

            var project = await projectService_.GetProject(projectId);

            if (project.Duration != 2)
            {
                Console.WriteLine("Invalid duration on project");
                return false;
            }

            var transactionLog = new Tracer.TraceTransactionLog()
            {
                Partition = 3
            };

            transactionLog.Transactions.Add(new Tracer.TraceTransaction()
            {
                Type = Tracer.TraceTransaction.Types.TraceTransactionType.CreateFile,
                TimeOffsetMs = 12,
                FilePath = "health"
            });

            await projectService_.AddTraceTransactionLog(projectId, transactionLog);

            var transactionLogs = await projectService_.GetTransactionLogsForRange(projectId, 0, 30000);
            if (!transactionLogs.Any())
            {
                Console.WriteLine("No transaction logs found");
                return false;
            }

            await projectService_.DeleteProject(projectId);

            return true;
        }

        private async Task<bool> TestLambda()
        {
            return await lambdaAccessService_.HealthCheck();
        }
    }
}