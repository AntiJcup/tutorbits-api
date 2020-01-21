using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateProjectModel : BaseCreateModel<Project>
    {
        public override Project Create()
        {
            return new Project()
            {
                DurationMS = 0
            };
        }
    }
}