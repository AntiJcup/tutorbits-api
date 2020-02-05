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

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerController : BaseCommentController<Answer, CreateAnswerModel, UpdateAnswerModel, AnswerViewModel>
    {
        protected override ICollection<Expression<Func<Answer, object>>> GetIncludes
        {
            get
            {
                return new List<Expression<Func<Answer, object>>>{
                    p => p.OwnerAccount,
                    p => p.Ratings,
                };
            }
        }

        public AnswerController(IConfiguration configuration,
                                DBDataAccessService dbDataAccessService,
                                AccountAccessService accountAccessService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
        }

        protected override async Task<bool> CanCreate(CreateAnswerModel createModel)
        {
            var existingModel = (await dbDataAccessService_.GetAllBaseModel(
                (Expression<Func<Answer, Boolean>>)(m => m.Owner == UserName && m.TargetId == createModel.TargetId)))
                .FirstOrDefault();

            return existingModel == null;
        }
    }
}
