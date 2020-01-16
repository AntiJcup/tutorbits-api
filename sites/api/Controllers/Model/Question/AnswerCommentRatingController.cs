using api.Models.Requests;
using api.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.Models.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerCommentRatingController : BaseRatingController<AnswerCommentRating, CreateAnswerCommentRatingModel, UpdateAnswerCommentRatingModel, AnswerCommentRatingViewModel>
    {
        public AnswerCommentRatingController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    AccountAccessService accountAccessService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
        }
    }
}
