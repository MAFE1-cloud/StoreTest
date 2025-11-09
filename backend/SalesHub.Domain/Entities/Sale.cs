using SalesHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesHub.Domain.Entities
{
    public class Sale : IEntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public string? ReceiptUrl { get; set; }


        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    }

}
