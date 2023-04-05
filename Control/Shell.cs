using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using variant1.Models;

namespace variant1.Control
{
    public static class Shell
    {
        const int defaultMoneyCount = 1000;
        static bool haveSupplies = false;

        public static List<Unit> UnitList;
        public static List<Product> Products;
        public static List<Buyer> Buyers;

        public static void InitializateShell()
        {
            Products = GetProducts();
            Buyers = GetBuyers();
            UnitList = GetUnits();
        }

        public static List<Buyer> GetBuyers()
        {
            string xml = "Resourse/Buyers.xml";
            string xmlString = File.ReadAllText(xml);


            // Parse XML data
            XElement root = XElement.Parse(xmlString);

            // Create Buyer objects from XML data
            List<Buyer> buyers = root.Elements("buyer")
                //.Take(10) // take first 10 elements only
                .Select(e => new Buyer
                {
                    Id = (int)e.Attribute("id"),
                    FullName = e.Value
                })
                .ToList();

            return buyers;
        }
        public static List<Supply> GetSupplyList()
        {
            try
            {
                string xml = "Resourse/Supplies.xml";
                string xmlString = File.ReadAllText(xml);

                //string xml = "XML-текст"; // здесь должен быть XML-текст, который нужно десериализовать

                List<Supply> supplies;

                XmlSerializer serializer = new XmlSerializer(typeof(List<Supply>), new XmlRootAttribute("ArrayOfSupply"));

                using (TextReader reader = new StringReader(xmlString))
                {
                    supplies = (List<Supply>)serializer.Deserialize(reader);
                    return supplies;
                }
            }
            catch { return new List<Supply>(); }
        }
        public static List<Unit> GetUnits()
        {
            string xml = "Resourse/Units.xml";
            string xmlString = File.ReadAllText(xml);


            // Parse XML data
            XElement root = XElement.Parse(xmlString);

            // Create Buyer objects from XML data
            List<Unit> units = root.Elements("Unit")
                //.Take(10) // take first 10 elements only
                .Select(e => new Unit
                {
                    Id = (int)e.Element("Id"),
                    UnitName = (string)e.Element("UnitName")
                })
                .ToList();

            return units;
        }
        public static List<Product> GetProducts()
        {
            try
            {
                string xml = "Resourse/Products.xml";
                string xmlString = File.ReadAllText(xml);

                // Parse XML data
                XElement root = XElement.Parse(xmlString);

                // Create Product objects from XML data
                List<Product> products = root.Elements("Product")
                    .Select(e => new Product
                    {
                        ProductId = (int)e.Element("ProductId"),
                        Name = (string)e.Element("Name"),
                        PurchasePrice = (int)e.Element("PurchasePrice"),
                        UnitId = (int)e.Element("UnitId"),
                    })
                    .ToList();

                return products;
            }
            catch { return new List<Product>(); }

        }

        public static Supply MakeSupply(int id)
        {
            Random rand = new Random();
            Product product = Products[rand.Next(Products.Count)];
            var result = new Supply
            {
                Count = rand.Next(50, 100),
                SupplyDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(rand.Next(5, 30)),
                Id = id,    /*supplyList.Count + 1,*/
                ProductId = product.ProductId,
                SalePrice = product.PurchasePrice + product.PurchasePrice / 100 * rand.Next(0, 20),
            };
            return result;
        }
        public static int CalculateRevenue()
        {
            try { 
            var sales = SalesShell.Sales;
            var revenue = sales.Select(s => s.Count *Shell.GetSupplyList().First(f=>f.Id==s.SupplyId).SalePrice).Sum();


            return revenue;
            }catch { return 0; }
        }
        public static int GetCurrentMoneyCount()
        {
            return GetSupplyList().Count == 0 ? defaultMoneyCount : 
                1000 - GetSupplyList()
                .Sum(s=>s.Count * Products
                .First(f=>f.ProductId==s.ProductId)
                .PurchasePrice) + CalculateRevenue();
        }
        public static void MakeSupplies(int iterationCount)
        {
            List<Supply> supplyList;
            if (File.Exists("Resourse/Supplies.xml"))
            {
                supplyList = GetSupplyList();
                haveSupplies = true;
            }
            else
            {
                supplyList = new List<Supply>();
            }
            //

            var moneyCount = GetCurrentMoneyCount();

            for (int i = 0; i < iterationCount; i++)
            {
                var supply = MakeSupply(supplyList.Count + 1);
                supplyList.Add(supply);
            }

            // создаем экземпляр XmlSerializer для типа List<Supply>
            XmlSerializer serializer = new XmlSerializer(typeof(List<Supply>));

            // создаем StringWriter для записи сериализованных данных в строку
            StringWriter stringWriter = new StringWriter();

            // сериализуем список supplies в формат XML и записываем результат в StringWriter
            serializer.Serialize(stringWriter, supplyList);

            //textBox.Text = stringWriter.ToString();

            using (FileStream fs = new FileStream("Resourse/Supplies.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, supplyList);
            }
        }
    }
}