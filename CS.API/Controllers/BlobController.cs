using CS.API.Models;
using CS.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CS.API.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json;
	using CS.API.Security;
    using CS.Common.Models;
    using System.Collections.Generic;

    [Route("v1/api/[controller]")]
	//[Authorize]
	public class BlobController : Controller
	{
		private readonly IAzureBlobService _azureBlobService;
		public BlobController(IAzureBlobService azureBlobService)
		{
			_azureBlobService = azureBlobService;
		}


		[HttpGet("testGet")]
		[AllowAnonymous]
		public async Task<IActionResult> TestGetBlobs()
		{
			var allBlobs = await _azureBlobService.ListAsync();
			return Ok(allBlobs);
		}

        #region Upload Image
        [HttpPost("uploadimage")]
		[AllowAnonymous]
		public async Task<IActionResult> UploadBlob([FromForm] FileUpload objFile)
		{
			//List<User> userList = JsonConvert.DeserializeObject<List<User>>(objFile.users);
            if(objFile.files.Count > 0)
            {
				var files = objFile.files;
				await _azureBlobService.UploadAsync(files);
				return Ok("Image successfully uploaded");
			}
			return null;
        }
		#endregion

		#region Delete Image
		[HttpPost("deleteimage")]
		[AllowAnonymous]
		public async Task<IActionResult> DeleteBlob(string fileUri)
		{
            try
            {
                await _azureBlobService.DeleteAsync(fileUri);
                return Ok("File was successfully deleted");
            }
            catch (Exception ex)
            {
				return null;
            }
        }
        #endregion
    }
}
