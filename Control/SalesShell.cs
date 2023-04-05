using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using variant1.Models;

namespace variant1.Control
{
    public static class SalesShell
    {
        public static List<Sales> Sales { get; set; } = new List<Sales>();

        public static List<Sales> GetSalesList() 
        {
            try
            {
                string xml = "Resourse/Sales.xml";
                string xmlString = File.ReadAllText(xml);

                //List<Supply> supplies;

                XmlSerializer serializer = new XmlSerializer(typeof(List<Sales>), new XmlRootAttribute("ArrayOfSales"));

                using (TextReader reader = new StringReader(xmlString))
                {
                    Sales = (List<Sales>)serializer.Deserialize(reader);
                    return Sales;
                }
            }
            catch {}

            return Sales; 
        }
        
        public static int GetAvailableCount(int supplyId)
        {
            var supply = Shell.GetSupplyList().FirstOrDefault(f => f.Id == supplyId);
            var count = supply.Count - Sales.Where(w => w.SupplyId == supplyId).Sum(s => s.Count);

            return count;
        }

        public static Sales MakeSale(int id)
        {
            Random rand = new Random();
            var availableCount = GetAvailableCount(id);
            if (availableCount > 0)
            {
                var sale = new Sales
                {
                    BuyerId = rand.Next(1, Shell.Buyers.Count),
                    Count = rand.Next(1, availableCount),
                    SaleDateTime = DateTime.Now,
                    Id = Sales.Count + 1,
                    SupplyId = id
                };
                return sale;
            }
            return null;
            throw new Exception();
        }
        public static void MakeSales(int iterrationCount)
        {
            if (Directory.Exists("Resourse/Sales.xml"))
            {
                Sales = DeserializeSalesFromXml();
            }

            Random rand = new Random();

            for (int i = 0; i < iterrationCount; i++)
            {
                var availableSupplies = Shell.GetSupplyList().Where(w => GetAvailableCount(w.Id) > 0).ToList();
                Sales s = MakeSale(availableSupplies[rand.Next(0, availableSupplies.Count)].Id);
                Sales.Add(s);
            }

            //textBox.Text=  SerializeSalesToXml(Sales);
            SerializeSalesToXml(Sales);
        }

        public static string SerializeSalesToXml(List<Sales> sales)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Sales>), new XmlRootAttribute("ArrayOfSales"));

            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, sales);
                string serializedXml = sw.ToString();

                using (FileStream fs = new FileStream("Resourse/Sales.xml", FileMode.Create))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(serializedXml);
                }

                return serializedXml;
            }
        }

        public static List<Sales> DeserializeSalesFromXml()
        {
            List<Sales> sales;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Sales>), new XmlRootAttribute("ArrayOfSales"));

            using (FileStream fs = new FileStream("Resourse/Sales.xml", FileMode.Open))
            {
                sales = (List<Sales>)serializer.Deserialize(fs);
            }

            return sales;
        }
    }
}
