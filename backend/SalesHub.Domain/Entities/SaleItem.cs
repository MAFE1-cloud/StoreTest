using SalesHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesHub.Domain.Entities
{
    public class SaleItem : IEntityBase
    { 
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

        public Product? Product { get; set; }
        public Sale? Sale { get; set; }
    }
}
