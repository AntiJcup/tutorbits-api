using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using Utils.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/project/streaming/[action]")]
    [ApiController]
    public class ProjectStreamingController : TutorBitsController
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        public ProjectStreamingController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        : base(configuration)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
        }

        [ActionName("project")]
        [HttpGet]
        public IActionResult GetProjectUrl([FromQuery]Guid projectId)
        {
            try
            {
                return new JsonResult(ProjectUrlGenerator.GenerateProjectUrl(projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("transactions")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionLogUrls([FromQuery]Guid projectId, [FromQuery]uint offsetStart, [FromQuery]uint offsetEnd)
        {
            try
            {
                var transactionLogFullPaths = await fileDataAccessService_.GetTransactionLogsForRange(projectId, offsetStart, offsetEnd);

                var transactionUrls = new Dictionary<string, string>();
                foreach (var transactionLogFullPath in transactionLogFullPaths)
                {
                    transactionUrls[transactionLogFullPath.Key] =
                        ProjectUrlGenerator.GenerateTransactionLogUrl(Path.GetFileName(transactionLogFullPath.Value), projectId, configuration_);
                }

                return new JsonResult(transactionUrls);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }


        [ActionName("download")]
        [HttpGet]
        public IActionResult GetProjectDownloadUrl([FromQuery]Guid projectId)
        {
            try
            {
                return new JsonResult(ProjectUrlGenerator.GenerateProjectDownloadUrl(projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("json")]
        [HttpGet]
        public IActionResult GetProjectJsonUrl([FromQuery]Guid projectId)
        {
            try
            {
                return new JsonResult(ProjectUrlGenerator.GenerateProjectJsonUrl(projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
