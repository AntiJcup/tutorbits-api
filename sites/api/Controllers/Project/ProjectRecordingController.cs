using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using GenericServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;
using Utils.Common;

namespace tutorbits_api.Controllers
{
    [Authorize]
    [Route("api/project/recording/[action]")]
    [ApiController]
    public class ProjectRecordingController : TutorBitsController
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        private readonly IConfiguration configuration_;

        public ProjectRecordingController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            configuration_ = configuration;
        }

        [ActionName("create")]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromQuery]Guid tutorialId)
        {
            try
            {
                //TODO guard this with auth and per account limits
                var tutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(tutorialId);
                if (tutorial == null)
                {
                    return BadRequest();
                }

                var project = new TraceProject()
                {
                    Id = tutorial.Id.ToString(),
                    PartitionSize = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                                                .GetValue<uint>(Constants.Configuration.Sections.Settings.PartitionSizeKey)
                };
                await fileDataAccessService_.CreateTraceProject(project);

                return new JsonResult(ProjectUrlGenerator.GenerateProjectUrl(tutorial.Id, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteProject([FromQuery]Guid tutorialId)
        {
            try
            {
                //TODO guard this with auth and per account limits
                var tutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(tutorialId);
                if (tutorial == null)
                {
                    return BadRequest();
                }

                await fileDataAccessService_.DeleteProject(tutorial.Id);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("add")]
        [HttpPost]
        public async Task<IActionResult> AddTransactionLog([FromQuery]Guid projectId)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                var tutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(projectId);
                if (tutorial == null)
                {
                    return BadRequest();
                }

                var transactionLog = TraceTransactionLog.Parser.ParseFrom(Request.Body);
                if (transactionLog == null || transactionLog.CalculateSize() == 0)
                {
                    return BadRequest();
                }
                Console.WriteLine(transactionLog.ToString());
                var transactionLogFullPath = await fileDataAccessService_.AddTraceTransactionLog(projectId, transactionLog);

                return new JsonResult(ProjectUrlGenerator.GenerateTransactionLogUrl(Path.GetFileName(transactionLogFullPath), projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
