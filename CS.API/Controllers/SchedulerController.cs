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
    public class SchedulerController : Controller
    {
        private IConfiguration Configuration;
        private ILogger<SchedulerController> Logger;
        private readonly AppSettings _appSettings;

        public SchedulerController(IConfiguration config, ILogger<SchedulerController> logger, IOptions<AppSettings> appSettings)
        {
            Configuration = config;
            Business.Database.Configuration = config;
            JwtSecurity.Configuration = config;
            Logger = logger;
            _appSettings = appSettings.Value;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> CreateFitting([FromBody] Fitting fitting)
        {
            try
            {
                if (fitting != null)
                {
                    var response = await SchedulerHandler.InsertFitting(fitting);
                    if (response != null)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return StatusCode(505, new ErrorResponse() { Message = "Error occurred while saving fitting to database, please try again." });
                    }
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Request - Fitting data is null" });
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
