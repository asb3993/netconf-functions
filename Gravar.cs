using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Acme.Carrinho
{
    public static class Gravar
    {
        [FunctionName("gravar")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string clientId = req.Query["clientId"];
                string itemId = req.Query["itemId"];

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(itemId))
                    return new BadRequestObjectResult("Por favor infome o clientId e o itemId");

                var carrinho = await Shared.BlobStorage.GetData(clientId);

                if (carrinho == null)
                    carrinho = new Dtos.Carrinho
                    {
                        ClientId = clientId,
                        Itens = new List<string>()
                    };

                carrinho.Itens.Add(itemId);
                await Shared.BlobStorage.StoreData(carrinho);

                return new OkObjectResult("");
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
