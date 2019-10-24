using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        private readonly IConfiguration configuration_;

        public RecordingController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            configuration_ = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromQuery]Guid tutorialId)
        {
            try
            {
                //TODO guard this with auth and per account limits
                var tutorial = await dbDataAccessService_.GetTutorial(tutorialId);
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

                return new JsonResult(new ProjectResponse()
                {
                    Id = project.Id,
                    ParitionSize = project.PartitionSize
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddTransactionLog([FromQuery]Guid projectId)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                var tutorial = await dbDataAccessService_.GetTutorial(projectId);
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
                await fileDataAccessService_.AddTraceTransactionLog(projectId, transactionLog);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
