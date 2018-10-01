using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Demo_LINQ_ClassOfProducts
{

    //
    // demo adapted from MSDN demo
    // https://code.msdn.microsoft.com/SQL-Ordering-Operators-050af19e/sourcecode?fileId=23914&pathId=1978010539
    //
    class Program
    {
        static void Main(string[] args)
        {
            //
            // write all data files to Data folder
            //
            GenerateDataFiles.InitializeXmlFile();

            List<Product> productList = ReadAllProductsFromXml();

            OrderByCatagory(productList);

            OrderByCatagoryAnoymous(productList);

            OrderByUnits(productList);

            OrderByPrice(productList);

            OrderByName(productList);

            OrderByTotalValue(productList);

            FindExpensive(productList);

            JasonsQuery(productList);

            NoahsQuery(productList);

            //
            // Write the following methods
            //

            // OrderByUnits(): List the names and units of all products with less than 10 units in stock. Order by units.

            // OrderByPrice(): List all products with a unit price less than $10. Order by price.

            // FindExpensive(): List the most expensive Seafood. Consider there may be more than one.

            // OrderByTotalValue(): List all condiments with total value in stock (UnitPrice * UnitsInStock). Sort by total value.

            // OrderByName(): List all products with names that start with "S" and calculate the average of the units in stock.

            // Query: Student Choice - Minimum of one per team member
        }

        private static void FindExpensive(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "The most expensive seafood(s)!");
            Console.WriteLine();

            var list = from o in products.Where(a => a.Category == "Seafood").OrderByDescending(a => a.UnitPrice).Take(1).Select(a => a.UnitPrice).Distinct()
                       from j in products
                       where o == j.UnitPrice && j.Category == "Seafood"
                       select new { Name = j.ProductName, Price = j.UnitPrice };

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(15));

            foreach (var product in list)
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Price.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByTotalValue(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "Condiments with total value in stock!");
            Console.WriteLine();

            var list = from o in products where o.UnitPrice * o.UnitsInStock > 0 && o.Category == "Condiments" orderby o.UnitPrice * o.UnitsInStock select new { Name = o.ProductName, Stock = Math.Round(o.UnitPrice * o.UnitsInStock) };

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Amount in Stock".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(15));

            foreach (var product in list)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Stock.ToString().PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Order products by inventory with less than 10 units in stock
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByUnits(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List the names and units of all priducts with less than 10 units in stock.");
            Console.WriteLine();

            var sortedProducts = products.Where(p => p.UnitsInStock < 10).OrderBy(p => p.UnitsInStock).Select(p => new {
                Name = p.ProductName,
                Inventory = p.UnitsInStock
            });

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Amount in Stock".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Inventory.ToString().PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Orders products by price
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByPrice(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List the products with a unit price of less than $10.");
            Console.WriteLine();

            var sortedProducts = products.Where(p => p.UnitPrice < 10).OrderBy(p => p.UnitPrice).Select(p => new {
                Name = p.ProductName,
                Price = p.UnitPrice
            });

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Unit Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByName(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List the products name that start with S.");
            Console.WriteLine();

            var sortedProducts = products.Where(p => p.ProductName.Contains("S")).Select(p => new {
                Name = p.ProductName,
                Price = p.UnitPrice
            });

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Unit Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void JasonsQuery(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "Group by most expensive. (Jason's Query)");
            Console.WriteLine();

            var list = from o in products.OrderByDescending(a => a.UnitPrice).GroupBy(s => s.Category).Select(a => a.First()).Distinct()
                       select new { Name = o.ProductName, Price = o.UnitPrice, Catagory = o.Category };

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Catagory".PadRight(40) + "Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(40) + "------------".PadRight(40) + "-------------".PadLeft(15));

            foreach (var product in list)
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Catagory.PadRight(40) + product.Price.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void NoahsQuery(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "Order by product id, descending.");
            Console.WriteLine();

            var sortedProducts = products.OrderByDescending(p => p.ProductID).Select(p => new {
                Name = p.ProductName,
                ID = p.ProductID
            });

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Product ID".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.ID.ToString().PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// read all products from an XML file and return as a list of Product
        /// in descending order by price
        /// </summary>
        /// <returns>List of Product</returns>
        private static List<Product> ReadAllProductsFromXml()
        {
            string dataPath = @"Data\Products.xml";
            List<Product> products;

            try
            {
                StreamReader streamReader = new StreamReader(dataPath);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>), new XmlRootAttribute("Products"));

                using (streamReader)
                {
                    products = (List<Product>)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return products;
        }

        /// <summary>
        /// Orders products by category
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByCatagory(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages"
                orderby product.UnitPrice descending
                select product;

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages").OrderByDescending(p => p.UnitPrice);

            Console.WriteLine(TAB + "Category".PadRight(15) + "Product Name".PadRight(25) + "Unit Price".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "------------".PadRight(25) + "----------".PadLeft(10));

            foreach (Product product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Category.PadRight(15) + product.ProductName.PadRight(25) + product.UnitPrice.ToString("C2").PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Orders products by anon category
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByCatagoryAnoymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages that cost more the $15 and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages" &&
                    product.UnitPrice > 15
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                };

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages" && p.UnitPrice > 15).OrderByDescending(p => p.UnitPrice).Select(p => new
            //{
            //    Name = p.ProductName,
            //    Price = p.UnitPrice
            //});


            decimal average = products.Average(p => p.UnitPrice);

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Product Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(20) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Average Price:".PadRight(20) + average.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }




    }
}
