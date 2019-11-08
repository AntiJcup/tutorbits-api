using TutorBits.Models.Common;

namespace api.Models.Views
{
    public abstract class BaseViewModel<TBaseModel> where TBaseModel : BaseModel
    {
        public abstract void Convert(TBaseModel baseModel);
    }
}