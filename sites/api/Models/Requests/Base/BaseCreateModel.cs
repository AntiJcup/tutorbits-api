using TutorBits.Models.Common;

namespace api.Models.Updates
{
    public abstract class BaseCreateModel<TBaseModel> where TBaseModel : BaseModel, new()
    {
        public abstract TBaseModel Create();
    }
}