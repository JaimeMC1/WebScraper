using HtmlAgilityPack;
using CsvHelper;
using MongoDB.Driver;
using System.Globalization;

namespace WebScraper
{
    public class Program
    {
        // Defining a custom class to store the scraped data 
        public class Product
        {
            public string? Url { get; set; }
            public string? Image { get; set; }
            public string? Name { get; set; }
            public string? Price { get; set; }
        }

        public static void Main()
        {
            // Creating the connection to the MongoDB database
            MongoDBConnection dbConnection = new MongoDBConnection();
            var database = dbConnection.GetDatabase();

            // Creating the list that will keep the scraped data 
            var products = new List<Product>();

            // Creating a reference to the "Products" collection in MongoDB
            var productsCollection = database.GetCollection<Product>("Products");

            // Creating the HAP object 
            var web = new HtmlWeb();

            // Visiting the target web page 
            var document = web.Load("https://www.scrapingcourse.com/ecommerce/");

            // Getting the list of HTML product nodes 
            var productHTMLElements = document.DocumentNode.QuerySelectorAll("li.product");

            // Iterating over the list of product HTML elements 
            foreach (var productHTMLElement in productHTMLElements)
            {
                // Scraping logic 
                var url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes["href"].Value);
                var image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes["src"].Value);
                var name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2").InnerText);
                var price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price").InnerText);

                var product = new Product() { Url = url, Image = image, Name = name, Price = price };
                products.Add(product);
            }

            // Inserting the list of products into MongoDB
            try
            {
                productsCollection.InsertMany(products); // Inserta todos los productos de una sola vez
                Console.WriteLine("Products successfully inserted into MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting products into MongoDB: {ex.Message}");
            }
        }
    }
}
