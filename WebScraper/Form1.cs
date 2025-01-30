using MongoDB.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WebScraper
{
    public partial class Form1 : Form
    {
        // Defining a custom class to store the scraped data 
        public class Product
        {
            public string? Url { get; set; }
            public string? Image { get; set; }
            public string? Name { get; set; }
            public string? Price { get; set; }
        }

        public class TenisPlayer
        {
            public string? UserId { get; set; } // the id that apt provide in the url for each player
            public string? Name { get; set; } // The name of the player
            public string? Age { get; set; } // The age of the player
            public string? Country { get; set; } // The country of the player
            public string? Hand { get; set; } // The type of hit
        }

        public class TenisPlayerStats
        {
            public string? Year { get; set; }
            public string? Aces { get; set; }
            public string? DoubFaul { get; set; } // Double Faults
            public string? FSer { get; set; } // First serve
            public string? FSerPoW { get; set; } // First serve points won
            public string? SSerPoW { get; set; } // Second serve points won
            public string? BreakPoF { get; set; } // Break points faced
            public string? BreakPoS { get; set; } // Break points saved
            public string? ServiGamesP { get; set; } // Service games played
            public string? ServiGamesW { get; set; } // Service games won
            public string? TotServiPoW { get; set; } // Total service points won
            public string? FSerRetPoW { get; set; } // First serve return points won
            public string? SSerRetPoW { get; set; } // Second serve return points won
            public string? BreakPoO { get; set; } // Break points opportunities
            public string? BreakPoC { get; set; } // break points converted
            public string? RetGamesP { get; set; } // Return games played
            public string? RetGamesW { get; set; } // Return games won
            public string? RetPoW { get; set; } // Return points won
            public string? TotPoW { get; set; } // Total points won
        }

        public class TenisPlayerActivity
        {
            public int? MatchId { get; set; } // The id of the match
            public string? MatchUser1 { get; set; } // The players who played
            public string? MatchUser2 { get; set; } // The players who played
            public int? Winner { get; set; } // The player who won
            public string? Year { get; set; }
            public string? Score { get; set; }
            public string? SerRat { get; set; } // Serve Rating
            public string? Aces { get; set; }  // Aces
            public string? DoubFaul { get; set; } // Double Faults
            public string? FSer { get; set; } // First serve
            public string? FSerPoW { get; set; } // First serve points won
            public string? SSerPoW { get; set; } // Second serve points won
            public string? BreakPoS { get; set; } // Break points saved
            public string? ServiGamesP { get; set; } // Service games played
            public string? RetRat { get; set; } // Return Rating
            public string? FSerRetPoW { get; set; } // First serve return points won
            public string? SSerRetPoW { get; set; } // Second serve return points won
            public string? BreakPoC { get; set; } // break points converted
            public string? RetGamesP { get; set; } // Return games played
            public string? ServiPoW { get; set; } // Service Points Won
            public string? RetPoW { get; set; } // Return Points Won
            public string? TotPoW { get; set; } // Total Points Won
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void btnUpdProfile_Click(object sender, EventArgs e)
        {
            // Creating the connection to the MongoDB database
            MongoDBConnection dbConnection = new MongoDBConnection();
            var database = dbConnection.GetDatabase();

            // Configuración de Selenium con Chrome
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless"); // Opcional: ejecuta en modo sin interfaz gráfica

            IWebDriver driver = new ChromeDriver(options);

            // Agregamos un numero random para el retraso. (del thread)
            Random random = new Random();

            // Navegar a la página
            driver.Navigate().GoToUrl("https://www.atptour.com/es/players/carlos-alcaraz/a0e2/player-activity?matchType=Singles&year=2025&tournament=all");
            Thread.Sleep(random.Next(3000, 5000));

            // Creating the list that will keep the scraped data 
            var products = new List<Product>();

            // Esperar a que los productos carguen
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString() == "complete");

            // Comprobar si las cookies están presentes y hacer clic si es necesario
            var acceptButton = driver.FindElements(By.Id("onetrust-accept-btn-handler")).FirstOrDefault();
            if (acceptButton != null)
            {
                //acceptButton.Click(); // Hacer clic si el botón está presente
            }

            wait.Until(d => d.FindElements(By.CssSelector(".player_cards.atp_card")));

            // Obtener elementos de productos con Selenium
            var productElements = driver.FindElements(By.CssSelector(".player_cards.atp_card"));

            // Iterating over the list of product HTML elements 
            foreach (var productElement in productElements)
            {
                try
                {
                    string url = productElement.FindElement(By.CssSelector("a")).GetAttribute("href");
                    string image = productElement.FindElement(By.CssSelector("img")).GetAttribute("src");
                    string name = productElement.FindElement(By.CssSelector("h2")).Text;
                    string price = productElement.FindElement(By.CssSelector(".price")).Text;

                    products.Add(new Product() { Url = url, Image = image, Name = name, Price = price });
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Algunos elementos no se encontraron y se omitieron.");
                }
            }

            // Cerrar Selenium
            driver.Quit();

            // Insertar en MongoDB
            var productsCollection = database.GetCollection<Product>("Products");

            try
            {
                productsCollection.InsertMany(products);
                Console.WriteLine("Productos insertados correctamente en MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insertando productos: {ex.Message}");
            }
        }

        private void btnUpdMatch_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdStats_Click(object sender, EventArgs e)
        {

        }
    }
}
