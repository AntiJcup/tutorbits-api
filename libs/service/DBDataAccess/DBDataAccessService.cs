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
            return await dataLayer_.GetAll<TModel>(where, skip, take);
        }

        public async Task<TModel> GetBaseModel<TModel>(params object[] keys) where TModel : BaseModel, new()
        {
            return await dataLayer_.Get<TModel>(keys);
        }

        public async Task<TModel> GetBaseModel<TModel>(ICollection<object> keys) where TModel : BaseModel, new()
        {
            return await dataLayer_.Get<TModel>(keys);
        }

        public async Task<TModel> CreateBaseModel<TModel>(TModel model) where TModel : BaseModel, new()
        {
            return await dataLayer_.Create(model);
        }

        public async Task UpdateBaseModel<TModel>(TModel model) where TModel : BaseModel, new()
        {
            await dataLayer_.Update(model);
        }

        public async Task DeleteBaseModel<TModel>(TModel model) where TModel : BaseModel, new()
        {
            await dataLayer_.Delete(model);
        }

        public async Task DeleteBaseModel<TModel>(params object[] keys) where TModel : BaseModel, new()
        {
            await dataLayer_.Delete<TModel>(keys);
        }

        #region Tutorials

        public async Task<ICollection<Tutorial>> GetAllTutorialsForUser(Guid userId)
        {
            return await dataLayer_.GetAll<Tutorial>((t => t.UserId == userId), null, null);
        }

        public async Task UpdateTutorial(Tutorial tutorial)
        {
            await dataLayer_.Update(tutorial);
        }

        public async Task DeleteTutorial(Tutorial tutorial)
        {
            await dataLayer_.Delete(tutorial);
        }
        #endregion
    }
}