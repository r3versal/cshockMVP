using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CS.API.Services
{
	public interface IAzureBlobService
	{
		Task<IEnumerable<Uri>> ListAsync();
		Task DeleteAsync(string fileUri, string userId, string isResume);
		Task DeleteAllAsync();
		Task<string> UploadAsyncProductPhoto(IFormFileCollection files, string productVariantId);
		Task<IEnumerable<Uri>> GetCandidateMediaAsync(string userId, bool isResume);

	}

	public class AzureBlobService : IAzureBlobService
	{
		private readonly IAzureBlobConnectionFactory _azureBlobConnectionFactory;

		public AzureBlobService(IAzureBlobConnectionFactory azureBlobConnectionFactory)
		{
			_azureBlobConnectionFactory = azureBlobConnectionFactory;
		}

		public async Task DeleteAllAsync()
		{
			var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer();

			BlobContinuationToken blobContinuationToken = null;
			do
			{
				var response = await blobContainer.ListBlobsSegmentedAsync(blobContinuationToken);
				foreach (IListBlobItem blob in response.Results)
				{
					if (blob.GetType() == typeof(CloudBlockBlob))
						await ((CloudBlockBlob)blob).DeleteIfExistsAsync();
				}
				blobContinuationToken = response.ContinuationToken;
			} while (blobContinuationToken != null);
		}

		public async Task DeleteAsync(string fileUri, string userId, string isResume)
		{
			var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer();

			Uri uri = new UriBuilder(fileUri).Uri;

			string filename = Path.GetFileName(uri.AbsolutePath);
			string fileNameFinal;
			if (isResume == "0")
			{
				fileNameFinal = "candidates/" + userId + "/image/" + filename;
			}
			else
			{
				fileNameFinal = "candidates/" + userId + "/resume/" + filename;
			}

			var blob = blobContainer.GetBlockBlobReference(fileNameFinal);
			var deleted = await blob.DeleteIfExistsAsync();
		}

		public async Task<IEnumerable<Uri>> ListAsync()
		{
			var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer();
			var allBlobs = new List<Uri>();
			BlobContinuationToken blobContinuationToken = null;
			do
			{
				var response = await blobContainer.ListBlobsSegmentedAsync(blobContinuationToken);
				foreach (var blob in response.Results)
				{
					allBlobs.Add(blob.Uri);

				}
				blobContinuationToken = response.ContinuationToken;
			} while (blobContinuationToken != null);
			return allBlobs;
		}


		public async Task<IEnumerable<Uri>> GetCandidateMediaAsync(string userId, bool isResume)
		{
			var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer();
			var allBlobs = new List<Uri>();
			BlobContinuationToken blobContinuationToken = null;
			do
			{
				var response = await blobContainer.ListBlobsSegmentedAsync(blobContinuationToken);
				foreach (IListBlobItem blob in response.Results)
				{
					Console.WriteLine(blob.Container.Name);
					Console.WriteLine(blob.StorageUri);
					Console.WriteLine(blob.Uri);
					Console.WriteLine(blob.ToString());
				}
				blobContinuationToken = response.ContinuationToken;
			} while (blobContinuationToken != null);
			return allBlobs;
		}

		public async Task<string> UploadAsyncProductPhoto(IFormFileCollection files, string productVariantId)
		{
			var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer();
			var toReturn = "";

			for (int i = 0; i < files.Count; i++)
			{
				var blob = blobContainer.GetBlockBlobReference(GetRandomBlobNameProductPhoto(files[i].FileName, productVariantId));

				blob.Properties.ContentType = "image/jpeg";
				
				using (var stream = files[i].OpenReadStream())
				{
					await blob.UploadFromStreamAsync(stream);
				}
				toReturn = blob.StorageUri.PrimaryUri.ToString();
			}
			return toReturn;
		}

		private string GetRandomBlobNameProductPhoto(string filename, string productVariantId)
		{
			string ext;

			ext = Path.GetExtension(filename);
			return string.Format("productphotos/" + productVariantId, DateTime.Now.Ticks, Guid.NewGuid(), ext);
		}

		public async Task<string> UploadAsyncCompany(IFormFileCollection files, string userId)
		{
			var blobContainer = await _azureBlobConnectionFactory.GetBlobContainer();
			var toReturn = "";

			for (int i = 0; i < files.Count; i++)
			{
				var blob = blobContainer.GetBlockBlobReference(GetRandomBlobNameCompany(files[i].FileName, userId));
				blob.Properties.ContentType = "image/jpeg";
				using (var stream = files[i].OpenReadStream())
				{
					await blob.UploadFromStreamAsync(stream);
				}
				toReturn = blob.StorageUri.PrimaryUri.ToString();
			}
			return toReturn;
		}

		private string GetRandomBlobNameCompany(string filename, string userId)
		{
			string ext = Path.GetExtension(filename);
			return string.Format("company/" + userId + "/" + "{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
		}
	}
}

