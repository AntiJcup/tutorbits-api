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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TutorialController : BaseModelController<Tutorial, CreateUpdateTutorialModel, CreateUpdateTutorialModel, TutorialViewModel>
    {
        public TutorialController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
            : base(configuration, dbDataAccessService, fileDataAccessService)
        {

        }
    }
}
