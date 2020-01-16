using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Models.Requests;
using api.Models.Updates;
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
    public class BaseRatingController<TModel, TCreateModel, TUpdateModel, TViewModel> : BaseModelController<TModel, TCreateModel, TUpdateModel, TViewModel>
        where TModel : Rating, new()
        where TCreateModel : BaseCreateModel<TModel>
        where TUpdateModel : BaseUpdateModel<TModel>
        where TViewModel : BaseViewModel<TModel>, new()
    {
        public BaseRatingController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    AccountAccessService accountAccessService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetCountForTarget([FromQuery] BaseState state, [FromQuery] Guid targetId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityCount = await dbDataAccessService_.CountAllBaseModel(
                state == BaseState.Undefined ? (Expression<Func<TModel, Boolean>>)(m => m.TargetId == targetId) : (Expression<Func<TModel, Boolean>>)(m => m.Status == state && m.TargetId == targetId));

            return new JsonResult(entityCount);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetScoreForTarget([FromQuery] BaseState state, [FromQuery] Guid targetId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dbDataAccessService_.GetAllBaseModel(
                state == BaseState.Undefined ? (Expression<Func<TModel, Boolean>>)(m => m.TargetId == targetId) : (Expression<Func<TModel, Boolean>>)(m => m.Status == state && m.TargetId == targetId));

            var score = 1;
            foreach (var rating in entities)
            {
                score += rating.Score;
            }

            return new JsonResult(score);
        }
    }
}
