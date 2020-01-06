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
using TutorBits.Models.Common;
using TutorBits.Preview;
using TutorBits.Project;
using Utils.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/project/preview/[action]")]
    [ApiController]
    public class ProjectPreviewController : TutorBitsController
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly PreviewService previewService_;

        private readonly ProjectService projectService_;

        public ProjectPreviewController(IConfiguration configuration, DBDataAccessService dbDataAccessService, PreviewService previewService, ProjectService projectService)
         : base(configuration)
        {
            dbDataAccessService_ = dbDataAccessService;
            previewService_ = previewService;
            projectService_ = projectService;
        }

        [ActionName("load")]
        [HttpGet]
        public async Task<IActionResult> LoadPreview([FromQuery]Guid projectId, [FromQuery]uint offsetEnd)
        {
            try
            {
                var previewId = Guid.NewGuid().ToString();
                var project = await projectService_.GetProject(projectId);
                if (project == null)
                {
                    return BadRequest();
                }
                await previewService_.GeneratePreview(project, (int)offsetEnd, previewId, true);
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
        public async Task<IActionResult> GeneratePreview([FromQuery]Guid projectId, [FromQuery]uint offsetEnd, [FromQuery]Guid? baseProjectId)
        {
            try
            {
                if ((Request.Body == null || Request.ContentLength <= 0) && !baseProjectId.HasValue)
                {
                    return BadRequest();
                }

                var previewId = Guid.NewGuid().ToString();

                var transactionLogs = TraceTransactionLogs.Parser.ParseFrom(Request.Body);
                if ((transactionLogs == null || transactionLogs.CalculateSize() == 0) && !baseProjectId.HasValue)
                {
                    return BadRequest();
                }

                var tempID = projectId;
                var tempProject = new TraceProject()
                {
                    Id = projectId.ToString()
                };
                await previewService_.GeneratePreview(tempProject, (int)offsetEnd, previewId, transactionLogs, true, baseProjectId);
                return new JsonResult(ProjectUrlGenerator.GenerateProjectPreviewUrl(previewId, tempID, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("download")]
        [HttpPost]
        public async Task<IActionResult> DownloadPreview([FromQuery]Guid projectId, [FromQuery]uint offsetEnd, [FromQuery]Guid? baseProjectId)
        {
            try
            {
                if ((Request.Body == null || Request.ContentLength <= 0) && !baseProjectId.HasValue)
                {
                    return BadRequest();
                }

                var tutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(projectId);
                if (tutorial != null)
                {
                    if (tutorial.Status != BaseState.Inactive)
                    {
                        return BadRequest();
                    }
                }

                var previewId = Guid.NewGuid().ToString();

                var transactionLogs = TraceTransactionLogs.Parser.ParseFrom(Request.Body);
                if ((transactionLogs == null || transactionLogs.CalculateSize() == 0) && !baseProjectId.HasValue)
                {
                    return BadRequest();
                }

                var tempProject = new TraceProject()
                {
                    Id = projectId.ToString()
                };
                await previewService_.GeneratePreview(tempProject, (int)offsetEnd, previewId, transactionLogs, false, baseProjectId);
                await previewService_.PackagePreviewZIP(projectId, previewId);

                return new JsonResult(ProjectUrlGenerator.GenerateProjectDownloadUrl(projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
