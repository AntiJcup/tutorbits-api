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
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace api.Controllers.Model
{
    public enum Action
    {
        Create,
        Update,
    }

    public abstract class BaseModelController<TModel, TCreateModel, TUpdateModel, TViewModel> : ControllerBase
        where TModel : BaseModel, new()
        where TCreateModel : BaseConvertableModel<TModel>
        where TUpdateModel : BaseConvertableModel<TModel>
        where TViewModel : BaseViewModel<TModel>, new()
    {
        protected readonly DBDataAccessService dbDataAccessService_;
        protected readonly FileDataAccessService fileDataAccessService_;
        protected readonly IConfiguration configuration_;

        public BaseModelController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            configuration_ = configuration;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            var keys = await GetKeysFromRequest();
            var entity = await dbDataAccessService_.GetBaseModel<TModel>(keys);
            var viewModel = new TViewModel();
            viewModel.Convert(entity);
            await EnrichViewModel(viewModel);
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

            var entity = await dbDataAccessService_.GetBaseModel<TModel>(id);
            var viewModel = new TViewModel();
            viewModel.Convert(entity);
            await EnrichViewModel(viewModel);
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
                take);
            var viewModels = new List<TViewModel>();

            foreach (var entity in entities)
            {
                var viewModel = new TViewModel();
                viewModel.Convert(entity);
                await EnrichViewModel(viewModel);
                viewModels.Add(viewModel);
            }
            return new JsonResult(viewModels);
        }


        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = createModel.Convert();
            await EnrichModel(model, Action.Create);
            await dbDataAccessService_.CreateBaseModel(model);
            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Update([FromBody] TUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = updateModel.Convert();
            await EnrichModel(model, Action.Update);
            await dbDataAccessService_.UpdateBaseModel(model);
            return Ok();
        }

        [HttpGet]
        public virtual async Task<IActionResult> UpdateStatusById([FromQuery] Guid id, [FromQuery] BaseState status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<TModel>(id);
            model.Status = status;
            await dbDataAccessService_.UpdateBaseModel(model);

            return Ok();
        }

        [HttpGet]
        public virtual async Task<IActionResult> Delete()
        {
            var keys = await GetKeysFromRequest();
            await dbDataAccessService_.DeleteBaseModel<TModel>(keys);
            return Ok();
        }

        [HttpGet]
        public virtual async Task<IActionResult> DeleteById([FromQuery] Guid id)
        {
            var keys = await GetKeysFromRequest();
            if (keys.Count() > 1)
            {
                return BadRequest("Error: Object has more than one key use Delete");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await dbDataAccessService_.DeleteBaseModel<TModel>(id);
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

        }

        protected virtual async Task EnrichViewModel(TViewModel viewModel)
        {

        }
    }
}