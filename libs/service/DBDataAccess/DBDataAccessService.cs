using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutorBits.Models.Common;

namespace TutorBits.DBDataAccess
{
    public class DBDataAccessService
    {
        private readonly DBDataLayerInterface dataLayer_;

        public DBDataAccessService(DBDataLayerInterface dataLayer)
        {
            dataLayer_ = dataLayer;
        }

        public async Task<ICollection<TModel>> GetAllBaseModel<TModel>(Expression<Func<TModel, Boolean>> where = null, int? skip = null, int? take = null) where TModel : BaseModel, new()
        {
            return await dataLayer_.GetAll<TModel, object>(new List<Expression<Func<TModel, object>>>{
                p => p.OwnerAccount
            }, where, skip, take);
        }

        public async Task<ICollection<TModel>> GetAllOwnedModel<TModel>(string userName, int? skip = null, int? take = null) where TModel : BaseModel, new()
        {
            return await dataLayer_.GetAll<TModel>((Expression<Func<TModel, Boolean>>)(m => m.Owner == userName && m.Status != BaseState.Deleted), skip, take);
        }

        public async Task<TModel> GetBaseModel<TModel>(params object[] keys) where TModel : BaseModel, new()
        {
            return await dataLayer_.Get<TModel, object>(new List<Expression<Func<TModel, object>>>{
                p => p.OwnerAccount
            }, keys);
        }

        public async Task<TModel> GetBaseModel<TModel>(ICollection<object> keys) where TModel : BaseModel, new()
        {
            return await dataLayer_.Get<TModel, object>(new List<Expression<Func<TModel, object>>>{
                p => p.OwnerAccount
            }, keys);
        }

        public async Task<TModel> CreateBaseModel<TModel>(TModel model) where TModel : BaseModel, new()
        {
            return await dataLayer_.Create(model);
        }

        public async Task UpdateBaseModel<TModel>(TModel model) where TModel : BaseModel, new()
        {
            model.DateModified = DateTime.Now;
            await dataLayer_.Update(model);
        }

        public async Task DeleteBaseModel<TModel>(TModel model) where TModel : BaseModel, new()
        {
            model.Status = BaseState.Deleted;
            model.DateModified = DateTime.Now;
            await dataLayer_.Update(model);
        }

        public async Task DeleteBaseModel<TModel>(params object[] keys) where TModel : BaseModel, new()
        {
            var model = await dataLayer_.Get<TModel>(keys);
            if (model == null)
            {
                throw new Exception("Model doesnt exist");
            }
            await DeleteBaseModel(model);
        }
    }
}