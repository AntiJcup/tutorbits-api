using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public abstract class CreateCommentModel<TCommentType> : BaseCreateModel<TCommentType> where TCommentType : Comment, new()
    {
        [Required]
        public Guid TargetId { get; set; }

        [MinLength(4)]
        [MaxLength(128)]
        [Required]
        public string Title { get; set; }

        [MaxLength(1028)]
        [MinLength(3)]
        public string Body { get; set; }

        protected TCommentType BaseCreate()
        {
            return new TCommentType()
            {
                TargetId = TargetId,
                Title = Title,
                Body = Body,
            };
        }
    }
}