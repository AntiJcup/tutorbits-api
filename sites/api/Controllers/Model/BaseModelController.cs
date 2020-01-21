using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Models.Updates;
using api.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using TutorBits.DBDataAccess;
using TutorBits.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using TutorBits.AccountAccess;

namespace api.Controllers.Model
{
    public enum Action
    {
        Create,
        Update,
    }

    public abstract class BaseModelController<TModel, TCreateModel, TUpdateModel, TViewModel> : TutorBitsController
        where TModel : BaseModel, new()
        where TCreateModel : BaseCreateModel<TModel>
        where TUpdateModel : BaseUpdateModel<TModel>
        where TViewModel : BaseViewModel<TModel>, new()
    {
        protected readonly DBDataAccessService dbDataAccessService_;
        protected readonly AccountAccessService accountAccessService_;

        protected virtual ICollection<Expression<Func<TModel, object>>> GetIncludes
        {
            get
            {
                return new List<Expression<Func<TModel, object>>>{
                    p => p.OwnerAccount
                };
            }
        }

        public BaseModelController(
            IConfiguration configuration,
            DBDataAccessService dbDataAccessService,
            AccountAccessService accountAccessService)
         : base(configuration)
        {
            dbDataAccessService_ = dbDataAccessService;
            accountAccessService_ = accountAccessService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            var keys = await GetKeysFromRequest();
            var entity = await dbDataAccessService_.GetBaseModel<TModel>(keys, GetIncludes);
            var viewModel = new TViewModel();
            viewModel.Convert(entity);
            await EnrichViewModel(viewModel, entity);
            return new JsonResult(viewModel);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var keys = await GetKeysFromRequest();
            if (keys.Count() > 1)
            {
                return BadRequest("Error: Object has more than one key use Get");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await dbDataAccessService_.GetBaseModel<TModel>(GetIncludes, id);
            var viewModel = new TViewModel();
            viewModel.Convert(entity);
            await EnrichViewModel(viewModel, entity);
            return new JsonResult(viewModel);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll([FromQuery] BaseState state, [FromQuery] int? skip = null, [FromQuery] int? take = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dbDataAccessService_.GetAllBaseModel(
                state == BaseState.Undefined ? (Expression<Func<TModel, Boolean>>)null : (Expression<Func<TModel, Boolean>>)(m => m.Status == state),
                skip,
                take,
                GetIncludes);
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

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> GetAllByOwner([FromQuery] int? skip = null, [FromQuery] int? take = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dbDataAccessService_.GetAllOwnedModel<TModel>(
                UserName,
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

        [HttpGet]
        public virtual async Task<IActionResult> CountAll([FromQuery] BaseState state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityCount = await dbDataAccessService_.CountAllBaseModel(
                state == BaseState.Undefined ? (Expression<Func<TModel, Boolean>>)null : (Expression<Func<TModel, Boolean>>)(m => m.Status == state));

            return new JsonResult(entityCount);
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = createModel.Create();
            await EnrichModel(model, Action.Create);
            var entity = await dbDataAccessService_.CreateBaseModel(model);
            var filledOutEntity = await dbDataAccessService_.GetBaseModel<TModel>(await GetKeysFromModel(entity), GetIncludes);
            await OnCreated(createModel, filledOutEntity);
            var viewModel = new TViewModel();
            viewModel.Convert(entity);
            await EnrichViewModel(viewModel, filledOutEntity);
            return new JsonResult(viewModel);
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Update([FromBody] TUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var modelKeys = updateModel.GetKeys();
            var model = await dbDataAccessService_.GetBaseModel<TModel>(modelKeys, GetIncludes);

            if (model == null)
            {
                return NotFound(); //Update cant be called on items that dont exist
            }

            if (!HasAccessToModel(model))
            {
                return Forbid(); //Only the owner and admins can modify this data
            }

            updateModel.Update(model);

            await EnrichModel(model, Action.Update);
            await dbDataAccessService_.UpdateBaseModel(model);
            await OnUpdated(updateModel, model);

            var viewModel = new TViewModel();
            viewModel.Convert(model);
            await EnrichViewModel(viewModel, model);

            return new JsonResult(viewModel);
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> UpdateStatusById([FromQuery] Guid id, [FromQuery] BaseState status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<TModel>(null, id);
            if (model == null)
            {
                return NotFound(); //Update cant be called on items that dont exist
            }

            if (!HasAccessToModel(model))
            {
                return Forbid(); //Only the owner and admins can modify this data
            }

            model.Status = status;
            await dbDataAccessService_.UpdateBaseModel(model);
            await OnUpdated(null, model);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Delete()
        {
            var keys = await GetKeysFromRequest();
            var oldModel = await dbDataAccessService_.GetBaseModel<TModel>(keys);

            if (oldModel == null)
            {
                return NotFound(); //Delete cant be called on items that dont exist
            }

            if (!HasAccessToModel(oldModel))
            {
                return Forbid(); //Only the owner and admins can delete this data
            }

            await dbDataAccessService_.DeleteBaseModel<TModel>(keys);
            await OnDeleted(oldModel);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> DeleteById([FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oldModel = await dbDataAccessService_.GetBaseModel<TModel>(null, id);

            if (oldModel == null)
            {
                return NotFound(); //Delete cant be called on items that dont exist
            }

            if (!HasAccessToModel(oldModel))
            {
                return Forbid(); //Only the owner and admins can delete this data
            }

            await dbDataAccessService_.DeleteBaseModel<TModel>(oldModel);
            await OnDeleted(oldModel);
            
            return Ok();
        }

        protected virtual async Task<ICollection<object>> GetKeysFromRequest()
        {
            var keys = new List<object>();

            var keyProperties = typeof(TModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)));
            foreach (var keyProp in keyProperties)
            {
                StringValues val;
                if (Request.Query.TryGetValue(keyProp.Name, out val))
                {
                    keys.Add(val.FirstOrDefault());
                }
            }

            if (keys.Count() < keyProperties.Count())
            {
                throw new Exception("Not enough keys");
            }

            return keys;
        }

        protected virtual async Task<ICollection<object>> GetKeysFromModel(TModel model)
        {
            var keys = new List<object>();

            var keyProperties = typeof(TModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)));
            foreach (var keyProp in keyProperties)
            {
                var val = keyProp.GetValue(model);
                if (val != null)
                {
                    keys.Add(val);
                }
                else
                {
                    throw new Exception("Key missing value");
                }
            }

            if (keys.Count() < keyProperties.Count())
            {
                throw new Exception("Not enough keys");
            }

            return keys;
        }

        protected virtual async Task<string> GenerateModelStateErrorMessage()
        {
            var builder = new StringBuilder();
            foreach (var modelValue in ModelState.Values)
            {
                foreach (var modelError in modelValue.Errors)
                {
                    builder.AppendLine($"{modelError.ErrorMessage}");
                }
            }

            return builder.ToString();
        }

        protected virtual async Task EnrichModel(TModel model, Action action)
        {
            switch (action)
            {
                case Action.Create:
                    model.Owner = UserName;
                    model.OwnerAccountId = (await accountAccessService_.GetAccount(UserName)).Id;
                    break;
            }
        }

        protected virtual async Task EnrichViewModel(TViewModel viewModel, TModel entity)
        {
            viewModel.Owner = entity.OwnerAccount == null ? null : entity.OwnerAccount.NickName;
        }

        protected virtual async Task OnCreated(TCreateModel createModel, TModel entity)
        {
            //Override when you need to something special on model create
        }

        protected virtual async Task OnDeleted(TModel entity)
        {
            //Override when you need to something special on model delete
        }

        protected virtual async Task OnUpdated(TUpdateModel updateModel, TModel entity)
        {
            //Override when you need to something special on model delete
        }
    }
}