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

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        protected readonly DBDataAccessService dbDataAccessService_;
        protected readonly FileDataAccessService fileDataAccessService_;
        protected readonly LambdaAccessService lambdaAccessService_;

        public HealthController(IConfiguration configuration,
                                DBDataAccessService dbDataAccessService,
                                FileDataAccessService fileDataAccessService,
                                LambdaAccessService lambdaAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
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

            var lookedUpTutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(tutorial.Id);
            if (tutorial.Title != lookedUpTutorial.Title || lookedUpTutorial.Title != "HEalthCheckTest")
            {
                Console.WriteLine("Invalid title on tutorial");
                return false;
            }

            lookedUpTutorial.Description = "HealthCheckTesting";
            await dbDataAccessService_.UpdateBaseModel(lookedUpTutorial);

            lookedUpTutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(tutorial.Id);
            if (lookedUpTutorial.Description != "HealthCheckTesting")
            {
                Console.WriteLine("Invalid description on tutorial");
                return false;
            }

            await dbDataAccessService_.DeleteBaseModel<Tutorial>(tutorial.Id);

            return true;
        }

        private async Task<bool> TestFileSystem()
        {
            var projectId = Guid.NewGuid();
            while (true)
            {
                try
                {
                    if (!(await fileDataAccessService_.DoesProjectExist(projectId)))
                    {
                        break;
                    }
                    projectId = Guid.NewGuid();
                }
                catch (Exception e)
                {
                    break;
                }
            }

            await fileDataAccessService_.CreateTraceProject(new Tracer.TraceProject()
            {
                Id = projectId.ToString(),
                Duration = 2,
                PartitionSize = 30
            });

            var project = await fileDataAccessService_.GetProject(projectId);

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

            await fileDataAccessService_.AddTraceTransactionLog(projectId, transactionLog);

            var transactionLogs = await fileDataAccessService_.GetTransactionLogsForRange(projectId, 0, 30000);
            if (!transactionLogs.Any())
            {
                Console.WriteLine("No transaction logs found");
                return false;
            }

            await fileDataAccessService_.DeleteProject(projectId);

            return true;
        }

        private async Task<bool> TestLambda()
        {
            return await lambdaAccessService_.HealthCheck();
        }
    }
}