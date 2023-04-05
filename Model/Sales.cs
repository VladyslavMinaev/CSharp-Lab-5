using System;

namespace variant1.Models
{
    public class Sales
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int SupplyId { get; set; }
        public int BuyerId { get; set; }
        public DateTime SaleDateTime { get; set; }
    }
}