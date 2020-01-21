using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using api.Models.Requests;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.Models.Common;
using TutorBits.Preview;
using TutorBits.Project;
using Utils.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectController : BaseModelController<Project, CreateProjectModel, UpdateProjectModel, ProjectViewModel>
    {
        private readonly ProjectService projectService_;
        private readonly PreviewService previewService_;

        public ProjectController(IConfiguration configuration,
                            DBDataAccessService dbDataAccessService,
                            AccountAccessService accountAccessService,
                            ProjectService projectService,
                            PreviewService previewService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
            projectService_ = projectService;
            previewService_ = previewService;
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Publish([FromQuery] Guid projectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<Project>(null, projectId);
            if (model == null)
            {
                return NotFound(); //Update cant be called on items that dont exist
            }

            if (!HasAccessToModel(model))
            {
                return Forbid(); //Only the owner and admins can modify this data
            }

            if (model.Status != BaseState.Inactive)
            {
                return BadRequest();
            }

            var project = await projectService_.GetProject(projectId);
            if (project == null)
            {
                return BadRequest();
            }

            //Finalize project
            var previewId = Guid.NewGuid().ToString();
            var previewDictionary = await previewService_.GeneratePreview(project, (int)project.Duration, previewId, false);
            await previewService_.PackagePreviewZIP(projectId, previewId);
            await previewService_.PackagePreviewJSON(projectId, previewDictionary);

            //Update model
            model.Status = BaseState.Active;
            await dbDataAccessService_.UpdateBaseModel(model);
            return Ok();
        }

        [Authorize]
        [ActionName("reset")]
        [HttpPost]
        public async Task<IActionResult> ResetProject([FromQuery]Guid projectId)
        {
            try
            {
                var project = await dbDataAccessService_.GetBaseModel<Project>(null, projectId);
                if (project == null)
                {
                    return NotFound();
                }

                if (!HasAccessToModel(project))
                {
                    return Forbid(); //Only the owner and admins can modify this data
                }

                //Prevent reseting a published project
                if (project.Status != BaseState.Inactive)
                {
                    return BadRequest();
                }

                await projectService_.ResetProject(project.Id);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [Authorize]
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

                var project = await dbDataAccessService_.GetBaseModel<Project>(null, projectId);
                if (project == null)
                {
                    return NotFound();
                }

                if (!HasAccessToModel(project))
                {
                    return Forbid(); //Only the owner and admins can modify this data
                }

                if (project.Status != BaseState.Inactive)
                {
                    return BadRequest();
                }

                var transactionLog = TraceTransactionLog.Parser.ParseFrom(Request.Body);
                if (transactionLog == null || transactionLog.CalculateSize() == 0)
                {
                    return BadRequest();
                }
                Console.WriteLine(transactionLog.ToString());
                var transactionLogFullPath = await projectService_.AddTraceTransactionLog(projectId, transactionLog);

                return new JsonResult(ProjectUrlGenerator.GenerateTransactionLogUrl(Path.GetFileName(transactionLogFullPath), projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [Authorize]
        [ActionName("addResource")]
        [HttpPost]
        public async Task<IActionResult> AddResource([FromQuery]Guid projectId, [FromQuery]string resourceFileName)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0 || string.IsNullOrWhiteSpace(resourceFileName))
                {
                    return BadRequest();
                }

                var project = await dbDataAccessService_.GetBaseModel<Project>(null, projectId);
                if (project == null)
                {
                    return BadRequest();
                }

                if (!HasAccessToModel(project))
                {
                    return Forbid(); //Only the owner and admins can modify this data
                }

                if (project.Status != BaseState.Inactive)
                {
                    return BadRequest();
                }

                var resourceId = Guid.NewGuid();
                using (var memoryStream = new MemoryStream())
                {
                    await Request.Body.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await projectService_.AddResource(projectId, memoryStream, resourceId);
                }

                return new JsonResult(resourceId.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("addResourceAnon")]
        [HttpPost]
        public async Task<IActionResult> AddResourceAnon([FromQuery]Guid projectId, [FromQuery]string resourceFileName)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0 || string.IsNullOrWhiteSpace(resourceFileName))
                {
                    return BadRequest();
                }

                var project = await dbDataAccessService_.GetBaseModel<Project>(null, projectId);
                if (project != null)
                {
                    return Forbid();
                }

                var resourceId = Guid.NewGuid();
                using (var memoryStream = new MemoryStream())
                {
                    await Request.Body.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await projectService_.AddResource(projectId, memoryStream, resourceId);
                }

                return new JsonResult(resourceId.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
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
                var transactionLogFullPaths = await projectService_.GetTransactionLogsForRange(projectId, offsetStart, offsetEnd);

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

        [ActionName("resource")]
        [HttpGet]
        public IActionResult GetProjectResourceUrl([FromQuery]Guid projectId, [FromQuery]Guid resourceId)
        {
            try
            {
                return new JsonResult(ProjectUrlGenerator.GenerateResourceUrl(resourceId, projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        protected override async Task EnrichViewModel(ProjectViewModel viewModel, Project entity)
        {
            await base.EnrichViewModel(viewModel, entity);
            viewModel.Url = ProjectUrlGenerator.GenerateProjectUrl(entity.Id, configuration_);
        }

        protected override async Task OnCreated(CreateProjectModel createModel, Project model)
        {
            var project = new TraceProject()
            {
                Id = model.Id.ToString(),
                PartitionSize = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                                                .GetValue<uint>(Constants.Configuration.Sections.Settings.PartitionSizeKey)
            };

            await projectService_.CreateTraceProject(project);
        }
    }
}