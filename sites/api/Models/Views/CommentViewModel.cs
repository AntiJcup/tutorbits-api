using System;
using System.ComponentModel.DataAnnotations;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class CommentViewModel : BaseViewModel<Comment>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public override void Convert(Comment baseModel)
        {
            Id = baseModel.Id.ToString();
            Title = baseModel.Title;
            Body = baseModel.Body;
            Type = baseModel.CommentType.ToString();
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
        }
    }
}