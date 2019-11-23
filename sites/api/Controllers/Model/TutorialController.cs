using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using api.Controllers.Model;
using api.Models;
using api.Models.Requests;
using api.Models.Views;
using GenericServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.Models.Common;
using Utils.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TutorialController : BaseModelController<Tutorial, CreateUpdateTutorialModel, CreateUpdateTutorialModel, TutorialViewModel>
    {
        private readonly LambdaAccessService lambdaAccessService_;
        public TutorialController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    FileDataAccessService fileDataAccessService,
                                    LambdaAccessService lambdaAccessService)
            : base(configuration, dbDataAccessService, fileDataAccessService)
        {
            lambdaAccessService_ = lambdaAccessService;
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Publish([FromQuery] Guid tutorialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<Tutorial>(tutorialId);
            if (model == null)
            {
                return NotFound(); //Update cant be called on items that dont exist
            }

            if (!HasAccessToModel(model))
            {
                return Forbid(); //Only the owner and admins can modify this data
            }

            if (model.Status != BaseState.Inactive)
            {
                return BadRequest();
            }

            var project = await fileDataAccessService_.GetProject(tutorialId);
            if (project == null)
            {
                return BadRequest();
            }

            await lambdaAccessService_.FinalizeProject(tutorialId);

            model.DurationMS = project.Duration;
            model.Status = BaseState.Active;
            await dbDataAccessService_.UpdateBaseModel(model);
            return Ok();
        }

        protected override async Task EnrichViewModel(TutorialViewModel viewModel)
        {
            viewModel.ThumbnailUrl = ProjectUrlGenerator.GenerateProjectThumbnailUrl(Guid.Parse(viewModel.Id), configuration_);
        }
    }
}
