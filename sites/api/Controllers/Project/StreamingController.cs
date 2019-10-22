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
    public class StreamingController : ControllerBase
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        private readonly IConfiguration configuration_;

        public StreamingController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            configuration_ = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetProject([FromQuery]Guid projectId)
        {
            try
            {
                var project = await fileDataAccessService_.GetProject(projectId);
                if (project == null || project.CalculateSize() == 0)
                {
                    return BadRequest();
                }

                return new JsonResult(new ProjectResponse()
                {
                    Id = project.Id,
                    ParitionSize = project.PartitionSize,
                    Duration = project.Duration
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionLogs([FromQuery]Guid projectId, [FromQuery]uint offsetStart, [FromQuery]uint offsetEnd)
        {
            try
            {
                var transactionLogFullPaths = await fileDataAccessService_.GetTransactionLogsForRange(projectId, offsetStart, offsetEnd);
                var transactionLogUrls = transactionLogFullPaths.Select(p => Utils.ProjectUrlGenerator.GenerateTransactionLogUrl(Path.GetFileName(p), projectId, configuration_));

                return new JsonResult(transactionLogUrls);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
