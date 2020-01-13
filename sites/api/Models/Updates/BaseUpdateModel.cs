using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Updates
{
    public abstract class BaseUpdateModel<TBaseModel> where TBaseModel : BaseModel, new()
    {
        public abstract ICollection<object> GetKeys();
        public abstract void Update(TBaseModel model);
    }
}