using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS.API
{
    using System.Text;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.IdentityModel.Tokens;
    using CS.API.Controllers;
    using CS.API.Services;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();
            services.AddSingleton<IAzureBlobService, AzureBlobService>();
            AddJwtBearerConfiguration(services);

            string connectionString = Configuration["ConnectionStrings:Development"];
            // Load the app settings
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            var test = loggerFactory.CreateLogger("test");
            test.LogWarning("test warning");
            app.UseAuthentication();
            app.UseMvc();
        }

        private void AddJwtBearerConfiguration(IServiceCollection services)
        {
            var secretKey = Configuration["JwtSecurity:SecretKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "CSAPI",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "User",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt => opt.TokenValidationParameters = tokenValidationParameters);
        }
    }
    public class AppSettings
    {
        public string DbConnectionString { get; set; }
    }
}
