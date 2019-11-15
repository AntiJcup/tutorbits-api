using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using TutorBits.Storage.MicrosoftSQL;
using Microsoft.EntityFrameworkCore;
using GenericServices.Setup;
using TutorBits.WindowsFileSystem;
using Microsoft.EntityFrameworkCore.Migrations;
using TutorBits.Lambda.Local;
using Amazon.S3;
using Amazon.Lambda;
using Amazon.ElasticTranscoder;
using TutorBits.S3FileSystem;
using TutorBits.Lambda.AWSLambda;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace tutorbits_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Env.IsEnvironment("Development"))
            {
                services.AddMvc(options =>
                {
                    options.Filters.Add(new AllowAnonymousFilter());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            }
            else
            {
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TutorBits", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("tutorbits",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            connectionString = connectionString.Replace("<UID>", Environment.GetEnvironmentVariable("SQL_UID"));
            connectionString = connectionString.Replace("<PWD>", Environment.GetEnvironmentVariable("SQL_PWD"));
            services.AddDbContext<TutorBitsSQLDbContext>(item => item.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("MicrosoftSQL"))
                );

            services.AddMicrosoftSQLDBDataAccessLayer();

            var useAWS = Configuration.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<bool>(Constants.Configuration.Sections.Settings.UseAWSKey, false);
            if (useAWS)
            {
                var awsOptions = Configuration.GetAWSOptions();
                services.AddDefaultAWSOptions(awsOptions);
                services.AddAWSService<IAmazonS3>();
                services.AddAWSService<IAmazonLambda>();
                services.AddAWSService<IAmazonElasticTranscoder>();
                services.AddS3FileDataAccessLayer();
                services.AddAWSLambdaAccessLayer();


                // The following 3 variables are null
                var userPoolId = Configuration.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<string>(Constants.Configuration.Sections.Settings.UserPoolIdKey);
                var userPoolClientId = Configuration.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<string>(Constants.Configuration.Sections.Settings.UserPoolClientIdKey);
                var userPoolAuthority = Configuration.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<string>(Constants.Configuration.Sections.Settings.UserPoolAuthorityKey);
                var userPoolClientSecret = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_CLIENT_SECRET");

                var amazonCognitoIdentityProvider = new AmazonCognitoIdentityProviderClient(awsOptions.Credentials,
                                                                                            awsOptions.Region);
                var cognitoUserPool = new CognitoUserPool(userPoolId, userPoolClientId, amazonCognitoIdentityProvider,
                                                        userPoolClientSecret);

                services.AddSingleton<IAmazonCognitoIdentityProvider>(x => amazonCognitoIdentityProvider);
                services.AddSingleton<CognitoUserPool>(x => cognitoUserPool);

                services.AddAuthentication("Bearer")
                    .AddJwtBearer(options =>
                    {
                        options.Audience = userPoolClientId;
                        options.Authority = userPoolAuthority;
                    });


                services.AddAuthorization(options =>
                {
                    options.AddPolicy("LimitedDomains", policy => policy.RequireClaim("custom:domain", "local.tutorbits.com"));
                });
            }
            else
            {
                services.AddWindowsFileDataAccessLayer();
                services.AddLocalLambdaAccessLayer();
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("tutorbits");

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
