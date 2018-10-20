using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Acme.Carrinho
{
    public static class ObterCarrinho
    {
        [FunctionName("obter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            Dtos.Carrinho carrinho = null;

            var clientId = req.Query["clientId"].ToString();

            if (!string.IsNullOrEmpty(clientId))
                carrinho = await Shared.BlobStorage.GetData(clientId);
            else 
                return new BadRequestObjectResult("Por favor informe o clientId.");

            return carrinho != null
                ? (ActionResult)new OkObjectResult(carrinho)
                : new NotFoundObjectResult("Carrinho não encontrado.");
        }
    }
}
