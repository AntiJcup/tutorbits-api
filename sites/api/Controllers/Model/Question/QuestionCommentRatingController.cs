﻿using api.Models.Requests;
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
    public class QuestionCommentRatingController : BaseRatingController<QuestionCommentRating, CreateQuestionCommentRatingModel, UpdateQuestionCommentRatingModel, QuestionCommentRatingViewModel>
    {
        public QuestionCommentRatingController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    AccountAccessService accountAccessService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
        }
    }
}
