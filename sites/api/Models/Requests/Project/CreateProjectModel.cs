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
        [Required]
        public string ProjectType { get; set; }

        public override Project Create()
        {
            return new Project()
            {
                DurationMS = 0,
                ProjectType = (ProjectType)Enum.Parse(typeof(ProjectType), ProjectType)
            };
        }
    }
}