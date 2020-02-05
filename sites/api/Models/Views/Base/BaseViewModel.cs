using TutorBits.Models.Common;

namespace api.Models.Views
{
    public abstract class BaseViewModel<TBaseModel> where TBaseModel : BaseModel
    {
        public string Owner { get; set; }

        public string OwnerId { get; set; }

        public double DateCreated { get; set; }

        public abstract void Convert(TBaseModel baseModel);
    }
}