
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EVR.API.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json;
	using CS.API.Security;
	using CS.Common.Models;
	using System.Collections.Generic;
	using Microsoft.Extensions.Logging;
	using CS.Business.Handlers;
	using Microsoft.Extensions.Configuration;
	using Newtonsoft.Json.Linq;
    using CS.API.Services;
    using CS.API.Controllers;
    using CS.API;
    using CS.API.Models;

    [Route("v1/api/[controller]")]
	[Authorize]
	public class BlobController : Controller
	{
		private readonly IAzureBlobService _azureBlobService;
		private IConfiguration Configuration;
		private ILogger<ProductController> Logger;
		private readonly AppSettings _appSettings;

		public BlobController(IAzureBlobService azureBlobService, IConfiguration config, ILogger<ProductController> logger, IOptions<AppSettings> appSettings)
		{
			_azureBlobService = azureBlobService;
			Configuration = config;
            CS.Business.Database.Configuration = config;
			JwtSecurity.Configuration = config;
			Logger = logger;
			_appSettings = appSettings.Value;
		}

		#region Test Get Blobs
		[HttpGet("testGet")]
		[AllowAnonymous]
		public async Task<IActionResult> TestGetBlobs()
		{
			var allBlobs = await _azureBlobService.ListAsync();
			return Ok(allBlobs);
		}
		#endregion

		#region Upload Blob Candidate
		[HttpPost("upload-media-productphoto")]
		public async Task<IActionResult> UploadBlobProductImage([FromForm] FileUpload objFile)
		{
			try
			{
				if (objFile.files.Count > 0)
				{
					var data = objFile.users;

					ProductPhoto item = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductPhoto>(data);

					Guid userId = item.productVariantId;
					item.productPhotoId = Guid.NewGuid();
					var files = objFile.files;
					string fileName = files[0].FileName;

					string uri = await _azureBlobService.UploadAsyncProductPhoto(files, userId.ToString());
					item.PhotoURL = uri;
					//Create candidatemediafilemapping row call candidate handler pass uri
					await ProductHandler.CreateProductPhotoMediaFile(item);
					return Ok("Image successfully uploaded");
				}
				else
				{
					return StatusCode(505, "EVR API Error: No files were recieved for upload, upload failed");
				}
			}
			catch (Exception ex)
			{
				string errorMessage = handleCatch(ex);
				return StatusCode(505, errorMessage);
			}
		}
		#endregion

		//#region Delete Image
		//[HttpPost("delete-media-candidate")]
		//public async Task<IActionResult> DeleteBlob([FromBody] CMFMDeleteRequest cmfm)
		//{

		//	if (cmfm != null)
		//	{
		//		Guid userId = cmfm.userId;
		//		string isResume = cmfm.isResume;

		//		try
		//		{
		//			var existingMediaFile = await CandidateHandler.CheckCandidateMediaFile(userId, isResume);
		//			if (existingMediaFile != null)
		//			{
		//				await CandidateHandler.DeleteCandidateMediaFile(userId, isResume);
		//				await _azureBlobService.DeleteAsync(existingMediaFile.MediafileLink, userId.ToString(), isResume);
		//				return Ok("File was successfully deleted");
		//			}
		//			return Ok("Existing file not found");

		//		}
		//		catch (Exception ex)
		//		{
		//			string errorMessage = handleCatch(ex);
		//			return StatusCode(505, errorMessage);
		//		}
		//	}
		//	return StatusCode(505, "EVR API Message: Missing fileUri, unable to delete image");
		//}
		//#endregion

		#region Handle Catch (Logs error in logger, sends email to Admin and returns error 505)
		private string handleCatch(Exception ex)
		{
			var st = new StackTrace(ex, true);
			var frame = st.GetFrame(0);
			var line = frame.GetFileLineNumber();
			string m = "EVR API Exception: " + ex.Message + "Error Line: " + line;
			Logger.LogError(m);

			return m;
		}
		#endregion
	}
}
