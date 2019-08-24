using System;
using System.Collections.Generic;

namespace HttpTriggerQueue
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> Produtos { get; set; }
        public string Email { get; set; }
        public decimal ValorTotal { get; set; }
    }

}