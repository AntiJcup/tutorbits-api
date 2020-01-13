using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateCommentModel : BaseCreateModel<Comment>
    {
        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(1028)]
        public string Body { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public Guid TargetTutorialId { get; set; }

        public override Comment Create()
        {
            return new Comment()
            {
                Title = Title,
                CommentType = (CommentType)Enum.Parse(typeof(CommentType), Type),
                Body = Body,
                TargetId = TargetTutorialId
            };
        }
    }
}