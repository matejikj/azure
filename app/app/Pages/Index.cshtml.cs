using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace app.Pages
{
	public class IndexModel : PageModel
	{

		// TODO: Hodnoty by bylo správné dát do konfigurace. Pro přehlednost a mít vše na jednom místě za účelem studia, nechávám tuto zjednodušenou variantu.
		private const string AzureStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=storagematejik;AccountKey=NprqIlhv+oZnA2aZv8cdwIqJSD8O8xTt6isF7j//4FAK7qZqPoZbP7pZ4MIK/LumtDTOmFtLB1a/ClI4+rc36A==;EndpointSuffix=core.windows.net";
		private const string AzureStorageContainerName = "matejikcontainer";
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}
		public List<BlobItem>? Blobs { get; set; }

		public void OnGet()
		{
			Blobs = CreateClient().GetBlobs().ToList();
		}

		public FileStreamResult OnGetDownloadBlob(string blobName)
		{
			this._logger.LogInformation("getting blobs.");
			var blobClient = CreateClient().GetBlobClient(blobName);
			var contentType = blobClient.GetProperties().Value.ContentType;
			var stream = blobClient.OpenRead(); // není třeba řešit Dispose - stream nám uzavře infrastruktura Razor Pages.
			return new FileStreamResult(stream, contentType);
		}

		private BlobContainerClient CreateClient()
		{
			return new Azure.Storage.Blobs.BlobContainerClient(AzureStorageConnectionString, AzureStorageContainerName);
		}
	}
}