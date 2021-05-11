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
    using Newtonsoft.Json;
    using CS.API.Security;

    [Route("v1/api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private IConfiguration Configuration;
        private ILogger<UserController> Logger;
        private readonly AppSettings _appSettings;

        public UserController(IConfiguration config, ILogger<UserController> logger, IOptions<AppSettings> appSettings)
        {
            Configuration = config;
            
            Business.Database.Configuration = config;
            JwtSecurity.Configuration = config;
            Logger = logger;
            _appSettings = appSettings.Value;
        }

        #region SignupUser
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignupUser([FromBody]SignupRequest request)
        {
            try
            {
                if (request != null)
                {
                    bool emailAvailable = await UserHandler.EmailNotInUse(request.User.Email);
                    if (!emailAvailable)
                    {
                        return StatusCode(505, new ErrorResponse() { Message = "There is already an account associated with that email address" });
                    }
                    var user = await UserHandler.InsertUser(request.User, request.Password);
                    if (user == null)
                    {
                        return Unauthorized();
                    }
                    Logger.LogWarning("User added");
                    return Ok(user);
                }
                return StatusCode(404);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Authenticate
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest request)
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