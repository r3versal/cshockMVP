#region Using Directives
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CS.Business.Handlers;
using CS.Common.Models;
using CS.API.Models;
#endregion

namespace CS.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Options;
    using CS.API.Security;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [Route("v1/api/[controller]")]
    [Authorize]
    public class AdminController : Controller
    {
        private IConfiguration Configuration;
        private ILogger<AdminController> Logger;
        private readonly AppSettings _appSettings;

        public AdminController(IConfiguration config, ILogger<AdminController> logger, IOptions<AppSettings> appSettings)
        {
            Configuration = config;
            Business.Database.Configuration = config;
            JwtSecurity.Configuration = config;
            Logger = logger;
            _appSettings = appSettings.Value;
        }

        #region Authenticate
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            try
            {
                if (request != null)
                {
                    var user = await UserHandler.Authenticate(request);
                    if (user == null)
                    {
                        Logger.LogInformation("User authentication failed: " + request.Email);
                        return Unauthorized();
                    }
                    Logger.LogInformation("User authenticated");
                    var jwt = JwtSecurity.GenerateToken(user.UserId, user.Email);

                    var loginResponse = new
                    {
                        User = user,
                        CSToken = jwt
                    };
                    var response = JsonConvert.SerializeObject(loginResponse, new JsonSerializerSettings { Formatting = Formatting.None });

                    return Ok(loginResponse);
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Login Request" });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion
    }
}