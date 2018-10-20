using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Acme.Carrinho.Shared
{
    public static class BlobStorage
    {
        private const string BlobConnectionString = "DefaultEndpointsProtocol=https;AccountName=netconfstorage;AccountKey=nM0uHubZuth56RdEZs/mNMg8A9cVgPfKYc9HjHsN5LwasIRY4VD9XeM0WuS0yBYjtNAZFfMHw8wGp8Gexq3TOw==;EndpointSuffix=core.windows.net";

        public static async Task<Dtos.Carrinho> GetData(string clientId)
        {
            var storageAccount = CloudStorageAccount.Parse(BlobConnectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference($"carrinhos");

            await container.CreateIfNotExistsAsync(
                BlobContainerPublicAccessType.Blob,
                new BlobRequestOptions(),
                new OperationContext());

            var blob = container.GetBlockBlobReference($"{clientId}.json");
            blob.Properties.ContentType = "application/json";

            var exist = await blob.ExistsAsync();

            var blobContent = exist 
                            ? await blob.DownloadTextAsync()
                            : string.Empty;

            var carrinho = JsonConvert.DeserializeObject<Dtos.Carrinho>(blobContent);

            return carrinho;
        }

        public static async Task StoreData(Dtos.Carrinho carrinho)
        {
            var storageAccount = CloudStorageAccount.Parse(BlobConnectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference($"carrinhos");

            await container.CreateIfNotExistsAsync(
                BlobContainerPublicAccessType.Blob,
                new BlobRequestOptions(),
                new OperationContext());

            var blob = container.GetBlockBlobReference($"{carrinho.ClientId}.json");
            blob.Properties.ContentType = "application/json";

            await blob.UploadTextAsync(JsonConvert.SerializeObject(carrinho));
        }
    }

}