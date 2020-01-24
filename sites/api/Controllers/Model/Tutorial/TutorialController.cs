using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Models.Requests;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class TutorialController : BaseModelController<Tutorial, CreateTutorialModel, UpdateTutorialModel, TutorialViewModel>
    {
        private readonly PreviewService previewService_;
        private readonly ProjectService projectService_;

        protected override ICollection<Expression<Func<Tutorial, object>>> GetIncludes
        {
            get
            {
                return new List<Expression<Func<Tutorial, object>>>{
                    p => p.OwnerAccount,
                    p => p.Ratings,
                    p => p.Thumbnail,
                    p => p.Project,
                };
            }
        }

        public TutorialController(IConfiguration configuration,
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
        public virtual async Task<IActionResult> Publish([FromQuery] Guid tutorialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<Tutorial>(null, tutorialId);
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
                return BadRequest("Unable to edit");
            }

            if (!model.VideoId.HasValue || !model.ProjectId.HasValue || !model.ThumbnailId.HasValue)
            {
                return BadRequest("incomplete");
            }

            //Update tutorial model
            model.Status = BaseState.Active;
            await dbDataAccessService_.UpdateBaseModel(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetProgrammingTopics()
        {
            return new JsonResult(Enum.GetNames(typeof(ProgrammingTopic)));
        }

        [HttpGet]
        public async Task<IActionResult> CheckTitle([FromQuery] string title)
        {
            return new JsonResult(!(await dbDataAccessService_.GetAllBaseModel((Expression<Func<Tutorial, Boolean>>)(m => m.Title == title)))
                .Any());
        }

        protected override async Task EnrichViewModel(TutorialViewModel viewModel, Tutorial entity)
        {
            await base.EnrichViewModel(viewModel, entity);

            if (entity.Thumbnail != null)
            {
                viewModel.ThumbnailUrl = ProjectUrlGenerator.GenerateProjectThumbnailUrl(entity.Thumbnail.Id, configuration_);
            }

            if (entity.Ratings != null)
            {
                viewModel.Score = BaseRatingController<TutorialRating, CreateTutorialRatingModel, UpdateTutorialRatingModel, TutorialRatingViewModel>
                    .CalculateRatingScore(entity.Ratings);
            }

            if (entity.Project != null)
            {
                viewModel.DurationMS = entity.Project.DurationMS;
            }
        }
    }
}
