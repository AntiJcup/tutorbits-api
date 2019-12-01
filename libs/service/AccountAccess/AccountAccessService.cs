﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.AuthAccess;
using TutorBits.DBDataAccess;
using TutorBits.Models.Common;

namespace TutorBits.AccountAccess
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAccountService(this IServiceCollection services)
        {
            return services.AddTransient<AccountAccessService>();
        }
    }

    public class AccountAccessService
    {
        private AuthAccessService authService_;
        private DBDataAccessService dbService_;

        public AccountAccessService(AuthAccessService authService, DBDataAccessService dbService)
        {
            authService_ = authService;
            dbService_ = dbService;
        }

        public async Task<Account> GetAccount(string userId)
        {
            return (await dbService_.GetAllOwnedModel<Account>(userId, 0, 1)).FirstOrDefault();
        }

        public async Task<Account> GetAccount(Guid accountId)
        {
            return (await dbService_.GetBaseModel<Account>(accountId));
        }

        public async Task<Account> CreateAccount(User user, string nickName = null)
        {
            var account = new Account()
            {
                Owner = user.Name,
                Email = user.Email,
                NickName = !string.IsNullOrWhiteSpace(nickName) ? nickName : "",
                AcceptOffers = false,
                Status = BaseState.Active
            };

            return await dbService_.CreateBaseModel(account);
        }

        public async Task<Account> UpdateNickName(Account account, string nickName)
        {
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            account.NickName = nickName;
            if ((await dbService_.GetAllBaseModel((Expression<Func<Account, Boolean>>)(m => m.NickName == nickName && m.Status != BaseState.Deleted))).Any())
            {
                throw new Exception("Account name already taken");
            }

            await dbService_.UpdateBaseModel(account);
            return account;
        }
    }
}
