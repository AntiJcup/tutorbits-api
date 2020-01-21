using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateVideoModel : BaseCreateModel<Video>
    {
        public override Video Create()
        {
            return new Video()
            {
            };
        }
    }
}