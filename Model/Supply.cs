using System;

namespace variant1.Models
{
    public class Supply
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get;set; }
        public int SalePrice { get; set; }
        public DateTime SupplyDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
