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
    public class CustomerController : Controller
    {
        private IConfiguration Configuration;
        private ILogger<CustomerController> Logger;
        private readonly AppSettings _appSettings;

        public CustomerController(IConfiguration config, ILogger<CustomerController> logger, IOptions<AppSettings> appSettings)
        {
            Configuration = config;

            Business.Database.Configuration = config;
            JwtSecurity.Configuration = config;
            Logger = logger;
            _appSettings = appSettings.Value;
        }

        //#region Create Customer
        //[HttpPost("create")]
        //public async Task<IActionResult> CreateCustomer([FromBody] CustomerDataModel customerDataModel)
        //{
            //try
            //{
            //    if (customerDataModel != null)
            //    {
            //        var customer = await CustomerHandler.InsertCustomer(customerDataModel);
            //        if (customer == null)
            //        {
            //            return StatusCode(505, "An unexpected error has ocurred, unable to create Customer");
            //        }
            //        Logger.LogWarning("Customer added");
            //        return Ok(customer);
            //    }
            //    return StatusCode(404);
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError(ex.ToString());
            //    return StatusCode(505, ex.Message);
            //}
        //}
        //#endregion

        #region Create Customer
        [HttpPost("order")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder([FromBody] CustomerOrderDataModel customerDataModel)
        {
            try
            {
                CustomerOrderDataModel newCODM = new CustomerOrderDataModel();

                if (customerDataModel != null)
                {
                    var customer = await CustomerHandler.InsertCustomer(customerDataModel);
                    if (customer == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to create Customer");
                    }
                    
                    var measurements = await MeasurementsHandler.InsertMeasurments(customerDataModel, customer.MeasurementsId);
                    if (measurements == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to create Customer measurements");
                    }
                    else
                    {
                        newCODM.measurements = measurements;
                    }

                    var order = await CustomerHandler.InsertCustomerOrder(customerDataModel, customer.CustomerId);
                    if (measurements == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to complete order");
                    }
                    else
                    {
                        newCODM.orderItems = order;
                    }

                    return Ok(newCODM);
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

        #region Update Customer
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDataModel customerDataModel)
        {
            try
            {
                if (customerDataModel != null)
                {
                    var customer = await CustomerHandler.UpdateCustomer(customerDataModel);
                    if (customer == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to update Customer");
                    }
                    Logger.LogWarning("Customer updated");
                    return Ok(customer);
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

        #region Delete Customer
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCustomer([FromBody] CustomerDataModel customerDataModel)
        {
            try
            {
                if (customerDataModel != null)
                {
                    //delete Customer Listing Favorites Mapping
                    //delete Customer Listing Recommendations
                    //delete Customer MediaFileMapping
                    //delete Customer Store Favorites Mapping
                    //delete Measurements Profile
                    
                    var customerDeleted = await CustomerHandler.DeleteCustomer(customerDataModel.Customer);
                    if (customerDeleted == false)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to delete Customer");
                    }
                    Logger.LogWarning("Customer deleted");
                    return Ok(customerDeleted);
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

        #region Get Customer
        [HttpPost("getcustomer")]
        public async Task<IActionResult> GetCustomer([FromBody] string email)
        {
            try
            {
                if (email != null && email != string.Empty)
                {
                    var customer = await CustomerHandler.GetCustomer(email);
                    if (customer == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to read Customer");
                    }
                    Logger.LogWarning("Customer found");
                    return Ok(customer);
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

        #region Get Customer Profile
        [HttpPost("getcustomerprofile")]
        public async Task<IActionResult> GetCustomerProfile([FromBody] string email)
        {
            try
            {
                if (email != null && email != string.Empty)
                {
                    //get Customer Listing Favorites Mapping
                    //get Customer Listing Recommendations
                    //get Customer MediaFileMapping
                    //get Customer Store Favorites Mapping
                    //get Measurements Profile
                    //var customerProfile = await CustomerHandler.GetCustomerProfile(email);
                    //if (customerProfile == null)
                    //{
                    //    return StatusCode(505, "An unexpected error has ocurred, unable to read Customer");
                    //}
                    //Logger.LogWarning("Customer Profile found");
                    //return Ok(customerProfile);
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

    }

}
