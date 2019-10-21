using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using constants;
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
        public async Task<IActionResult> CreateProject([FromQuery]string name)
        {
            try
            {
                var tutorial = new Tutorial()
                {
                    Name = name
                };
                tutorial = await dbDataAccessService_.CreateTutorial(tutorial);

                var project = new TraceProject()
                {
                    Id = tutorial.Id.ToString(),
                    PartitionSize = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                                                .GetValue<uint>(Constants.Configuration.Sections.Settings.PartitionSizeKey)
                };
                await fileDataAccessService_.CreateTraceProject(project);

                return new JsonResult(new CreateProjectResponse()
                {
                    Id = project.Id
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task AddTransactionLog([FromQuery]Guid projectId)
        {
            try
            {
                var tutorial = await dbDataAccessService_.GetTutorial(projectId);
                var transactionLog = TraceTransactionLog.Parser.ParseFrom(Request.Body);
                Console.WriteLine(transactionLog.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
