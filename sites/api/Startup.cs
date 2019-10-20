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

namespace tutorbits_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
            services.AddDbContext<TutorBitsSQLDbContext>(item => item.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("MicrosoftSQL")));

            services.AddMicrosoftSQLDBDataAccessLayer();
            services.AddWindowsFileDataAccessLayer();
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
