﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.AuthAccess;
using TutorBits.Models.Common;

namespace TutorBits.Auth.AWSAuth
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAWSAuthAccessLayer(this IServiceCollection services)
        {
            services.AddTransient<AuthLayerInterface, AWSAuthAccessLayerInterface>();
            return services.AddTransient<AuthAccessService>();
        }
    }

    public class AWSAuthAccessLayerInterface : AuthLayerInterface
    {
        private readonly CognitoUserPool userPool_;
        private readonly IConfiguration configuration_;
        public AWSAuthAccessLayerInterface(IConfiguration config, CognitoUserPool userPool)
        {
            configuration_ = config;
            userPool_ = userPool;
        }

        public async Task<User> GetUser(string userName)
        {
            var user = userPool_.GetUser(userName);
            var userDetails = await user.GetUserDetailsAsync();

            return new User()
            {
                Name = userDetails.Username,
                Email = userDetails.UserAttributes.FirstOrDefault(a => a.Name == "email").Value
            };
        }
    }
}