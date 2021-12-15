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
        public async Task<IActionResult> CreateOrder([FromBody] CustomerOrderDataModel customerDataModel)
        {
            try
            {
                CustomerOrderDataModel newCODM = new CustomerOrderDataModel();

                if (customerDataModel != null)
                {
                    if (customerDataModel.isNewCustomer)
                    {
                        var customer = await CustomerHandler.InsertCustomer(customerDataModel);
                        if (customer == null)
                        {
                            return StatusCode(505, "An unexpected error has ocurred, unable to create Customer");
                        }
                        customerDataModel.userId = customer.UserId;
                        newCODM.userId = customer.UserId;

                        Measurements measurements = await MeasurementsHandler.UpdateMeasurements(customerDataModel.measurements);
                        if (measurements == null)
                        {
                            measurements = await MeasurementsHandler.InsertMeasurments(customerDataModel, customer.MeasurementsId, customerDataModel.measurements.UserId);

                            if (measurements == null)
                            {
                                return StatusCode(505, "An unexpected error has ocurred, unable to create Customer measurements");
                            }
                        }
                        else
                        {
                            newCODM.measurements = measurements;
                        }
                    }

                    if (customerDataModel.isGuest)
                    {
                        customerDataModel.order.isMemberCheckout = false;
                    }

                    if (customerDataModel.isExistingCustomer)
                    {

                        customerDataModel.order.isMemberCheckout = true;
                    }

                    var address = await AddressHandler.InsertAddress(customerDataModel);
                    if (address == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to insert address");
                    }
                    newCODM.shippingAddress = address;
                    customerDataModel.shippingAddress = address;
                    
                    var order = await CustomerHandler.InsertCustomerOrder(customerDataModel);
                    if (order == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to complete order");
                    }
                    else
                    {
                        newCODM.order = order;
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

        //#region Update Customer
        //[HttpPut("update")]
        //public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDataModel customerDataModel)
        //{
        //    try
        //    {
        //        if (customerDataModel != null)
        //        {
        //            var customer = await CustomerHandler.UpdateCustomer(customerDataModel);
        //            if (customer == null)
        //            {
        //                return StatusCode(505, "An unexpected error has ocurred, unable to update Customer");
        //            }
        //            Logger.LogWarning("Customer updated");
        //            return Ok(customer);
        //        }
        //        return StatusCode(404);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex.ToString());
        //        return StatusCode(505, ex.Message);
        //    }
        //}
        //#endregion

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
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] Measurements measurements)
        {
            try
            {
                if (measurements != null && measurements.UserId != Guid.Empty && measurements.UserId != null)
                {
                    var customerProfile = await MeasurementsHandler.UpsertMeasurements(measurements);

                    return Ok(customerProfile);
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
        [HttpGet("get-customer-profile")]
        public async Task<IActionResult> GetCustomerProfile([FromQuery] Guid userId)
        {
            try
            {
                if (userId != null && userId != Guid.Empty)
                {
                    //get Customer Listing Favorites Mapping
                    //get Customer Listing Recommendations
                    //get Customer MediaFileMapping
                    //get Customer Store Favorites Mapping
                    //get Measurements Profile
                    var customerProfile = await MeasurementsHandler.GetMeasurements(userId);
   
                    return Ok(customerProfile);
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

        #region Get Order History
        [HttpGet("order-history")]
        public async Task<IActionResult> OrderHistory([FromQuery] Guid userId)
        {
            try
            {
                if (userId != null && userId != Guid.Empty)
                {
                    var orderHistory = await OrderHandler.OrderHistory(userId);
                    if (orderHistory == null)
                    {
                        return StatusCode(505, "An unexpected error has ocurred, unable to complete order");
                    }

                    return Ok(orderHistory);
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
