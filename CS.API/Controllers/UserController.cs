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
    using System.Diagnostics;

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
                    var jwtRefresh = JwtSecurity.GenerateRefreshToken(user.UserId, user.Email);

                    var loginResponse = new
                    {
                        User = user,
                        CSToken = jwt,
                        RefreshToken = jwtRefresh
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

        #region Update User
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                if (user != null)
                {
                    var userUpdated = await UserHandler.UpdateUser(user);
                    if (userUpdated == null)
                    {
                        Logger.LogInformation("User update failed: " + user);
                        return StatusCode(505, "User update failed.");
                    }
                    return Ok(userUpdated);
                }
                return StatusCode(408, new ErrorResponse() { Message = "User Data not recieved." });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region RefreshToken
        [HttpPut("{userId}/refreshToken")]
        public async Task<IActionResult> RefreshToken([FromRoute] Guid userId, [FromHeader] string authorization, [FromBody] Device deviceInfo)
        {
            try
            {
                //Null checks
                if (authorization == null)
                {
                    return StatusCode(500, new ErrorResponse() { Message = "Authorization token is missing from header" });
                }
                if (deviceInfo.RefreshToken == null)
                {
                    return StatusCode(500, new ErrorResponse() { Message = "Refresh Token is missing from Json body" });
                }

                //Token verification
                var updatedAuthorization = authorization.Replace("Bearer ", "");
                var verifyToken = UserHandler.ReadToken(updatedAuthorization, userId);

                if (!verifyToken)
                {
                    return StatusCode(500, new ErrorResponse() { Message = "UserId does not match Authorized User associated with provided Auth Token" });
                }

                var userInfo = await UserHandler.GetUserById(userId);

                if (userInfo != null)
                {
                    var authUser = userInfo;
                    var verifyRefreshToken = UserHandler.ReadToken(deviceInfo.RefreshToken, userId);

                    if (verifyRefreshToken)
                    {
                        var jwt = JwtSecurity.GenerateToken(authUser.UserId, authUser.Email);
                        var jwtRefresh = JwtSecurity.GenerateRefreshToken(authUser.UserId, authUser.Email);
                        var loginResponse = new
                        {
                            User = authUser,
                            CSToken = jwt,
                            RefreshToken = jwtRefresh
                        };
                        var response = JsonConvert.SerializeObject(loginResponse, new JsonSerializerSettings { Formatting = Formatting.None });
                        return Ok(loginResponse);
                    }
                    return StatusCode(500, new ErrorResponse() { Message = "Device Refresh token is invalid" });
                }
                Logger.LogInformation("Unable to retrieve user information with the UserId provided");
                return StatusCode(401, new ErrorResponse() { Message = "Unable to retrieve user information with the UserId provided" });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Forgot Password Send Link
        [HttpPost("reset-password-request")]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordResetLink([FromBody] LoginRequest lr)
        {
            try
            {
                if (lr.Email != null)
                {
                    //Create hash and save to database
                    string email = lr.Email;
                    PasswordReset pwr = await UserHandler.PasswordResetHash(email);

                    var u = await UserHandler.UserByEmail(email);

                    if (u != null)
                    {
                        //Send password reset link email
                        //TODO: setup sendgrid for password reset emails
                        //var user = u.AsList()[0];
                        //MandrillHandler mh = new MandrillHandler();
                        //var emailResults = mh.PasswordResetLink(email, pwr.newHash, user.IsCandidate);
                        if (pwr != null)
                        {
                            return Ok(true);
                        }
                        else
                        {
                            return Ok(false);
                        }
                    }

                    else
                    {
                        return Ok(false);
                    }
                }
                return StatusCode(505, "CS API Error: Email Json data not received, unable to send password reset link");
            }
            catch (Exception ex)
            {
                string errorMessage = handleCatch(ex);
                return StatusCode(505, errorMessage);
            }
        }
        #endregion

        #region Check Hash From Password Reset Request
        [HttpGet("check-reset-password-link")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPasswordResetToken([FromQuery] string token)
        {
            try
            {
                if (token != null)
                {
                    PasswordReset pwr = await UserHandler.CheckPasswordResetHash(token);
                    if (pwr.UserId != null)
                    {
                        return Ok(pwr.UserId);
                    }
                    return Unauthorized();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = handleCatch(ex);
                return StatusCode(505, errorMessage);
            }
        }
        #endregion

        #region New Password
        [HttpPut("store-password")]
        [AllowAnonymous]
        public async Task<IActionResult> NewPasswordUser([FromBody] PasswordResetRequest prr)
        {
            try
            {
                if (prr.userId != null && prr.password != null)
                {
                    User user = new User();
                    user.UserId = prr.userId;
                    user.IsActive = true;
                    string newPassword = prr.password;

                    var updateUser = await UserHandler.UpdateUser(user, newPassword);
                    if (updateUser != null)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return Ok(false);
                    }

                }
                return StatusCode(505, "CS API Error: User or password Json data not received, unable to update password");
            }
            catch (Exception ex)
            {
                string errorMessage = handleCatch(ex);
                return StatusCode(505, errorMessage);
            }
        }
        #endregion

        #region Get User By Email
        [HttpPost("get-user-by-email")]
        public async Task<IActionResult> GetByEmail([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest != null)
                {
                    string email = loginRequest.Email;
                    var user = await UserHandler.UserByEmail(email);
                    if (user == null)
                    {
                        Logger.LogInformation("User authentication failed: " + email);
                        return Unauthorized();
                    }
                    return Ok(user);
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Login Request, no email recieved" });
            }
            catch (Exception ex)
            {
                string errorMessage = handleCatch(ex);
                return StatusCode(505, errorMessage);
            }
        }
        #endregion

        #region Handle Catch (Logs error in logger, sends email to Admin and returns error 505)
        private string handleCatch(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            string m = "CS API Exception: " + ex.Message + "Error Line: " + line;
            Logger.LogError(m);

            return m;
        }
        #endregion
    }
}