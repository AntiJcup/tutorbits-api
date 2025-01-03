﻿using System;
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
using TutorBits.Models.Common;

namespace tutorbits_api.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseModelController<Account, CreateAccountModel, UpdateAccountModel, AccountViewModel>
    {
        private readonly AuthAccessService authService_;

        public AccountController(
            IConfiguration configuration,
            DBDataAccessService dbDataAccessService,
            AccountAccessService accountAccessService,
            AuthAccessService authService)
           : base(configuration, dbDataAccessService, accountAccessService)
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
                //Use user name for nick name when it isn't an external login
                account = await accountAccessService_.CreateAccount(await authService_.GetUser(AccessToken), this.IsExternalLogin ? null : UserName);

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

        [HttpGet]
        public async Task<IActionResult> CheckNickName([FromQuery] string nickName)
        {
            return new JsonResult(!(await accountAccessService_.DoesNickNameExist(nickName)));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] CreateAccountModel createModel)
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

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public override async Task<IActionResult> UpdateStatusById([FromQuery] Guid id, [FromQuery] BaseState status)
        {
            return await base.UpdateStatusById(id, status);
        }
    }
}
