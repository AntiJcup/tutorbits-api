using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using api.Controllers;
using api.Models;
using api.Models.Views;
using GenericServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.AuthAccess;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using Utils.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : TutorBitsController
    {
        private readonly AuthAccessService authService_;

        public UserController(IConfiguration configuration, AuthAccessService authService) : base(configuration)
        {
            authService_ = authService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await authService_.GetUser(AccessToken);

            return new JsonResult(user);
        }
    }
}
