using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Models.Requests;
using api.Models.Views;
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
        }
    }
}
