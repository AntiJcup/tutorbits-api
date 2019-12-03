using System;
using System.Threading.Tasks;
using api.Models.Requests;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.Models.Common;
using Utils.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TutorialController : BaseModelController<Tutorial, CreateUpdateTutorialModel, CreateUpdateTutorialModel, TutorialViewModel>
    {
        private readonly LambdaAccessService lambdaAccessService_;
        public TutorialController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    FileDataAccessService fileDataAccessService,
                                    LambdaAccessService lambdaAccessService,
                                    AccountAccessService accountAccessService)
            : base(configuration, dbDataAccessService, fileDataAccessService, accountAccessService)
        {
            lambdaAccessService_ = lambdaAccessService;
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Publish([FromQuery] Guid tutorialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<Tutorial>(tutorialId);
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

            var project = await fileDataAccessService_.GetProject(tutorialId);
            if (project == null)
            {
                return BadRequest();
            }

            //Finalize project
            var previewId = Guid.NewGuid().ToString();
            var previewDictionary = await fileDataAccessService_.GeneratePreview(project, (int)project.Duration, previewId);
            await fileDataAccessService_.PackagePreviewZIP(tutorialId, previewId);
            await fileDataAccessService_.PackagePreviewJSON(tutorialId, previewDictionary);

            //Update tutorial model
            model.DurationMS = project.Duration;
            model.Status = BaseState.Active;
            await dbDataAccessService_.UpdateBaseModel(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetTutorialTypes()
        {
            return new JsonResult(Enum.GetNames(typeof(TutorialType)));
        }

        protected override async Task EnrichViewModel(TutorialViewModel viewModel, Tutorial entity)
        {
            await base.EnrichViewModel(viewModel, entity);
            viewModel.ThumbnailUrl = ProjectUrlGenerator.GenerateProjectThumbnailUrl(Guid.Parse(viewModel.Id), configuration_);
        }
    }
}
