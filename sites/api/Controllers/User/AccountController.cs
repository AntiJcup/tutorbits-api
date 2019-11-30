using System;
using System.Threading.Tasks;
using api.Controllers.Model;
using api.Models.Requests;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AccountAccess;
using TutorBits.AuthAccess;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.Models.Common;

namespace tutorbits_api.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "IsAdmin")]
    public class AccountController : BaseModelController<Account, CreateUpdateAccountModel, CreateUpdateAccountModel, AccountViewModel>
    {
        private readonly AuthAccessService authService_;

        public AccountController(
            IConfiguration configuration,
            DBDataAccessService dbDataAccessService,
            FileDataAccessService fileDataAccessService,
            LambdaAccessService lambdaAccessService,
            AccountAccessService accountAccessService,
            AuthAccessService authService)
           : base(configuration, dbDataAccessService, fileDataAccessService, accountAccessService)
        {
            authService_ = authService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var account = await accountAccessService_.GetAccount(UserName);
            if (account == null) //If user didnt exist before this create an account
            {
                account = await accountAccessService_.CreateAccount(await authService_.GetUser(UserName));

                if (account == null)
                {
                    return BadRequest();
                }
            }

            var accountView = new AccountViewModel();
            accountView.Convert(account);
            await EnrichViewModel(accountView, account);

            return new JsonResult(accountView);
        }


        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] CreateUpdateAccountModel createModel)
        {
            return await base.Create(createModel);
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public override async Task<IActionResult> GetAll([FromQuery] BaseState state, [FromQuery] int? skip = null, [FromQuery] int? take = null)
        {
            return await base.GetAll(state, skip, take);
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public override async Task<IActionResult> Get()
        {
            return await base.Get();
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public override async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            return await base.GetById(id);
        }
    }
}
