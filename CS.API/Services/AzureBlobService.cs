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
		Task<string> UploadAsyncProductPhoto(IFormFileCollection files, string productVariantId);
		Task<IEnumerable<Uri>> GetProductMediaAsync(string productVariantId);

	}

	public class AzureBlobService : IAzureBlobService
	{
		private readonly IAzureBlobConnectionFactory _azureBlobConnectionFactory;

		public AzureBlobService(IAzureBlobConnectionFactory azureBlobConnectionFactory)
		{
			_azureBlobConnectionFactory = azureBlobConnectionFactory;
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


		public async Task<IEnumerable<Uri>> GetProductMediaAsync(string productVariantId)
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
	}
}

