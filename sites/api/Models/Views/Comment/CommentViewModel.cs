using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public abstract class CommentViewModel<TCommentType> : BaseViewModel<TCommentType> where TCommentType : Comment
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Status { get; set; }

        public double DateCreated { get; set; }

        protected void BaseConvert(Comment baseModel)
        {
            Id = baseModel.Id.ToString();
            Title = baseModel.Title;
            Body = baseModel.Body;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
            DateCreated = baseModel.DateCreated.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
        }
    }
}