using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public abstract class UpdateCommentModel<TCommentType> : BaseUpdateModel<TCommentType>  where TCommentType : Comment, new()
    {
        [Required]
        public Guid Id { get; set; }

        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(1028)]
        public string Body { get; set; }

        protected ICollection<object> BaseGetKeys()
        {
            return new List<object> {
                Id
            };
        }

        protected void BaseUpdate(TCommentType model)
        {
            model.Title = Title;
            model.Body = Body;
        }
    }
}