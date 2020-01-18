using System;
using System.Collections.Generic;
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
    public class BaseCommentController<TModel, TCreateModel, TUpdateModel, TViewModel> : BaseModelController<TModel, TCreateModel, TUpdateModel, TViewModel>
        where TModel : Comment, new()
        where TCreateModel : BaseCreateModel<TModel>
        where TUpdateModel : BaseUpdateModel<TModel>
        where TViewModel : BaseViewModel<TModel>, new()
    {
        public BaseCommentController(IConfiguration configuration,
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
                state == BaseState.Undefined ?
                    (Expression<Func<TModel, Boolean>>)(m => m.TargetId == targetId) :
                    (Expression<Func<TModel, Boolean>>)(m => m.Status == state && m.TargetId == targetId));

            return new JsonResult(entityCount);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetCommentsForTarget([FromQuery] BaseState state, [FromQuery] Guid targetId, [FromQuery] int? skip = null, [FromQuery] int? take = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dbDataAccessService_.GetAllBaseModel(
                state == BaseState.Undefined ?
                    (Expression<Func<TModel, Boolean>>)(m => m.TargetId == targetId) :
                    (Expression<Func<TModel, Boolean>>)(m => m.Status == state && m.TargetId == targetId),
                skip,
                take);
            var viewModels = new List<TViewModel>();

            foreach (var entity in entities)
            {
                var viewModel = new TViewModel();
                viewModel.Convert(entity);
                await EnrichViewModel(viewModel, entity);
                viewModels.Add(viewModel);
            }
            return new JsonResult(viewModels);
        }

        protected override async Task EnrichModel(TModel model, Action action)
        {
            await base.EnrichModel(model, action);

            switch (action)
            {
                case Action.Create:
                    model.Status = BaseState.Active;
                    break;
            }
        }
    }
}
