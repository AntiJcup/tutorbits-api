using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateExampleCommentModel : CreateCommentModel<ExampleComment>
    {
        public override ExampleComment Create()
        {
            return BaseCreate();
        }
    }
}