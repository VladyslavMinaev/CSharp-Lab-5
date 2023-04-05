using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace variant1.Control
{
    public static class LogsGetter
    {
        public static string GetSupplyLogs()
        {
            string result = "";

            var supplies = Shell.GetSupplyList();
            foreach(var item in supplies)
            {
                result += $"{item.Id}: {Shell.GetProducts().First(f => f.ProductId == item.ProductId).Name}, " +
                    $"count: {item.Count}, {item.SalePrice} UAH\n";
            }
            return result;
        } 
        public static string GetSalesLogs()
        {
            string result = "";
            var sales = SalesShell.GetSalesList();
            var buyers = Shell.GetBuyers();
            //var units= Shell.GetUnits();
            foreach(var item in sales)
            {
                var fullname = buyers.FirstOrDefault(x => x.Id == item.BuyerId).FullName;
                result += $"{item.Id}: sales count: {item.Count}, {item.SaleDateTime}\n";
            }
            return result;
        }
    }
}
