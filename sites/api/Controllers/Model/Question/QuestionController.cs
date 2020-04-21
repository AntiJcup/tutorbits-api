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

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : BaseModelController<Question, CreateQuestionModel, UpdateQuestionModel, QuestionViewModel>
    {
        private readonly PreviewService previewService_;
        private readonly ProjectService projectService_;

        protected override ICollection<Expression<Func<Question, object>>> GetIncludes
        {
            get
            {
                return new List<Expression<Func<Question, object>>>{
                    p => p.OwnerAccount,
                    p => p.Ratings,
                    p => p.Answers
                };
            }
        }

        public QuestionController(IConfiguration configuration,
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
        public virtual async Task<IActionResult> Publish([FromQuery] Guid questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<Question>(GetIncludes, questionId);
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

            //Update Example model
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
            return new JsonResult(!(await dbDataAccessService_.GetAllBaseModel((Expression<Func<Question, Boolean>>)(m => m.Title == title)))
                .Any());
        }

        protected override async Task EnrichViewModel(QuestionViewModel viewModel, Question entity)
        {
            await base.EnrichViewModel(viewModel, entity);

            if (entity.Ratings != null)
            {
                viewModel.Score = BaseRatingController<QuestionRating, CreateQuestionRatingModel, UpdateQuestionRatingModel, QuestionRatingViewModel>
                    .CalculateRatingScore(entity.Ratings);
            }

            if (entity.Answers != null)
            {
                viewModel.AnswerCount = entity.Answers.Count();
            }
        }
    }
}
