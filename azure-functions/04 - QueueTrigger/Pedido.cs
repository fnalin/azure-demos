using System;
using System.Collections.Generic;

namespace QueueTrigger
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> Produtos { get; set; }

        public string ClienteId { get; set; }
        public string Email { get; set; }
        public decimal Valor { get; set; }
        public bool Aprovado { get; set; }
    }

}
