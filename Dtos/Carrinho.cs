using System.Collections.Generic;

namespace Acme.Carrinho.Dtos
{
    public class Carrinho
    {
        public string ClientId { get; set; }
        public List<string> Itens { get; set; }
    }
}