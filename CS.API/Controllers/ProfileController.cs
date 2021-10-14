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
    public class ProfileController : Controller
    {
        private IConfiguration Configuration;
        private ILogger<ProfileController> Logger;
        private readonly AppSettings _appSettings;

        public ProfileController(IConfiguration config, ILogger<ProfileController> logger, IOptions<AppSettings> appSettings)
        {
            Configuration = config;

            Business.Database.Configuration = config;
            JwtSecurity.Configuration = config;
            Logger = logger;
            _appSettings = appSettings.Value;
        }

        #region Get Profile By UserId
        [HttpGet("profile-by-userId")]
        public async Task<IActionResult> GetProfileByUserId([FromQuery] Guid userId)
        {
            try
            {
                if(userId != null)
                {
                    var profile = await ProfileHandler.GetProfile(userId);
                    if (profile == null)
                    {
                        return StatusCode(505, "Profile was not found");
                    }
                    else
                    {
                        var measurements = await MeasurementsHandler.GetMeasurements(userId);
                    }
                    Logger.LogWarning("Profile Found");
                    return Ok(profile);
                }
                return StatusCode(505, "Missing User Id");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Update Profile By UserId
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfileByUserId([FromBody] CustomerProfile profile)
        {
            try
            {
                if(profile != null)
                {
                    var profileUpdated = await ProfileHandler.UpdateProfile(profile);
                    if (profileUpdated == null)
                    {
                        return StatusCode(505, "Profile was not found or updated.");
                    }
                    Logger.LogWarning("Profile Found");
                    return Ok(profile);
                }
                return StatusCode(505, "Missing profile data.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Insert
        [HttpPost("create-profile")]
        public async Task<IActionResult> InsertProfileByUserId([FromBody] CustomerProfile profile)
        {
            try
            {
                if (profile != null)
                {
                    var profileUpdated = await ProfileHandler.InsertProfile(profile);
                    if (profileUpdated == null)
                    {
                        return StatusCode(505, "Profile was not found or updated.");
                    }
                    Logger.LogWarning("Profile Found");
                    return Ok(profile);
                }
                return StatusCode(505, "Missing profile data.");
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