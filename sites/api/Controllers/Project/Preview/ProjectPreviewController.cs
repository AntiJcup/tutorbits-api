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
    [Route("api/project/preview/[action]")]
    [ApiController]
    public class ProjectPreviewController : TutorBitsController
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        public ProjectPreviewController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
         : base(configuration)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
        }

        [ActionName("load")]
        [HttpGet]
        public async Task<IActionResult> LoadPreview([FromQuery]Guid projectId, [FromQuery]uint offsetEnd)
        {
            try
            {
                var previewId = Guid.NewGuid().ToString();
                var project = await fileDataAccessService_.GetProject(projectId);
                if (project == null)
                {
                    return BadRequest();
                }
                await fileDataAccessService_.GeneratePreview(project, (int)offsetEnd, previewId);
                return new JsonResult(ProjectUrlGenerator.GenerateProjectPreviewUrl(previewId, projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("generate")]
        [HttpPost]
        public async Task<IActionResult> GeneratePreview([FromQuery]uint offsetEnd, [FromQuery]Guid? baseProjectId)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                var previewId = Guid.NewGuid().ToString();

                var transactionLogs = TraceTransactionLogs.Parser.ParseFrom(Request.Body);
                if (transactionLogs == null || transactionLogs.CalculateSize() == 0)
                {
                    return BadRequest();
                }

                var tempID = Guid.NewGuid();
                var tempProject = new TraceProject()
                {
                    Id = tempID.ToString()
                };
                await fileDataAccessService_.GeneratePreview(tempProject, (int)offsetEnd, previewId, transactionLogs, baseProjectId);
                return new JsonResult(ProjectUrlGenerator.GenerateProjectPreviewUrl(previewId, tempID, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
