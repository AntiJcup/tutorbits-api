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
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using Utils.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : TutorBitsController
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        private readonly IConfiguration configuration_;

        private readonly CognitoUserPool userService_;

        public UserController(IConfiguration configuration, CognitoUserPool userService)
        {
            configuration_ = configuration;
            userService_ = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = userService_.GetUser(UserName);
            var userDetails = await user.GetUserDetailsAsync();
            var view = new UserViewModel();
            view.Convert(userDetails);

            return new JsonResult(view);
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetOtherUser([FromQuery] string userName)
        {
            var user = userService_.GetUser(userName);
            var userDetails = await user.GetUserDetailsAsync();
            var view = new UserViewModel();
            view.Convert(userDetails);

            return new JsonResult(view);
        }
    }
}
