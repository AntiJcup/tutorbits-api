using TutorBits.Models.Common;

namespace api.Models.Updates
{
    public abstract class BaseConvertableModel<TBaseModel> where TBaseModel : BaseModel, new()
    {
        public abstract TBaseModel Convert();
    }
}