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
    public class ProductController : Controller
    {
        private IConfiguration Configuration;
        private ILogger<AdminController> Logger;
        private readonly AppSettings _appSettings;

        public ProductController(IConfiguration config, ILogger<AdminController> logger, IOptions<AppSettings> appSettings)
        {
            Configuration = config;
            Business.Database.Configuration = config;
            JwtSecurity.Configuration = config;
            Logger = logger;
            _appSettings = appSettings.Value;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                if (product != null)
                {
                    var response = await ProductHandler.InsertProduct(product);
                    if (response != null)
                    {
                        return Ok(response);
                    }
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Request - Product data is null" });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (product != null)
                {
                    var response = await ProductHandler.UpdateProduct(product);
                    if (response != null)
                    {
                        return Ok(response);
                    }
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Request - Product data is null" });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpPut("delete")]
        public async Task<IActionResult> DeleteProduct([FromBody] Product product)
        {
            try
            {
                if (product != null)
                {
                    var response = await ProductHandler.DeleteProduct(product);
                    if (response != null)
                    {
                        return Ok(response);
                    }
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Request - Product data is null" });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Get All Products
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                List<Product> products = new List<Product>();
                var response = await ProductHandler.GetAllProducts();
                if (response != null)
                {
                    //get product photos
                    for (int i = 0; i < response.Count; i++)
                    {
                        Product prod = response[i];
                        var prodPhotos = await ProductHandler.GetProductPhotosByProductId(prod.ProductId);
                        prod.ProductPhotos = prodPhotos;
                        products.Add(prod);
                    }
                    return Ok(products);
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Get By Id
        [HttpGet("get-by-id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct([FromQuery] Guid productId)
        {
            try
            {
                if (productId != null)
                {
                    var response = await ProductHandler.GetProductById(productId);
                    if (response != null)
                    {
                        //get product photos
                        var response2 = await ProductHandler.GetProductPhotosByProductId(response.ProductVariantId);
                        response.ProductPhotos = response2;
                        return Ok(response);
                    }
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Request - Product Id is null" });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(505, ex.Message);
            }
        }
        #endregion

        #region Get By Id
        [HttpGet("get-by-variant-id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductVariant([FromQuery] Guid productVariantId)
        {
            try
            {
                Guid temp = Guid.NewGuid();

                if (productVariantId != null)
                {
                    var response = await ProductHandler.GetProductByVariantId(productVariantId);
                    if (response != null)
                    {
                        //get product photos
                        var response2 = await ProductHandler.GetProductPhotosByProductVariantId(response.ProductVariantId);
                        response.ProductPhotos = response2;
                        return Ok(response);
                    }
                }
                return StatusCode(408, new ErrorResponse() { Message = "Bad Request - Product Id is null" });
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