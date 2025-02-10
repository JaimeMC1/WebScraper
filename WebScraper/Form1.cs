using MongoDB.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Sprache;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static WebScraper.Form1;

namespace WebScraper
{
    public partial class Form1 : Form
    {
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
            public string? UserId { get; set; } // the id that apt provide in the url for each player
            public string? Year { get; set; }
            public string? Win { get; set; }
            public string? Lose { get; set; }
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
            public string? MatchId { get; set; } // The id of the match. got it from the href 61 26 64
            public string? MatchUser1 { get; set; } // The players who played. got it from the url
            public string? MatchUser2 { get; set; } // The players who played. got it from the href player against+
            public string? Tournament { get; set; }  // Aces
            public string? Rd { get; set; }  // Aces
            public string? Winner { get; set; } // The player who won. tick or x
            public string? Year { get; set; }
            public string? Set1 { get; set; }
            public string? Set2 { get; set; } // Serve Rating
            public string? Set3 { get; set; }  // Aces
            public string? Set4 { get; set; } // Double Faults
            public string? Set5 { get; set; }  // Aces
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

            ChromeDriver driver = new ChromeDriver(options);

            // Navegar a la página
            driver.Navigate().GoToUrl("https://www.atptour.com/es/players/carlos-alcaraz/a0e2/overview");

            // Creating the list that will keep the scraped data 
            var tenisPlayer = new List<TenisPlayer>();

            // Creamos el log
            DevStatsTxt.Text = "";

            // Esperar a que los productos carguen
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString() == "complete");
                DevStatsTxt.Text += "Carga Inicial SI completada\n";
            }
            catch (WebDriverTimeoutException)
            {
                DevStatsTxt.Text += "Carga Inicial NO completada\n";
            }

            // Comprobar si las cookies están presentes y hacer clic si es necesario
            int i = 0;
            while (i <= 5)
            {
                var acceptButton = driver.FindElements(By.Id("onetrust-accept-btn-handler")).FirstOrDefault();
                if (acceptButton != null)
                {
                    acceptButton.Click(); // Hacer clic si el botón está presente
                    DevStatsTxt.Text += "Cookies SI encontradas\n";
                    break;
                }
                else
                    DevStatsTxt.Text += "Cookies NO encontradas\n";
                Thread.Sleep(500);
                i++;
            }
            
            // Obtener elementos de productos con Selenium
            wait.Until(d => d.FindElements(By.CssSelector(".personal_details")));
            var productElements = driver.FindElements(By.CssSelector(".personal_details"));

            // Iterating over the list of product HTML elements 
            foreach (var productElement in productElements)
            {
                try
                {
                    // De la Url actual Divide la URL y extraer la parte entre el nombre y "overview"
                    string[] partsUrl = driver.Url.Split('/');
                    string playerId = partsUrl[partsUrl.Length - 2];

                    string userId = playerId;
                    DevStatsTxt.Text += "\nId: "+ userId;
                    string name = driver.FindElement(By.CssSelector(".player_name")).Text;
                    DevStatsTxt.Text += "\nName: " + name;
                    string age = productElement.FindElement(By.CssSelector(".pd_left li:first-child span:nth-child(2)")).Text.Split(' ')[0];
                    DevStatsTxt.Text += "\nAge: " + age;
                    string country = productElement.FindElement(By.CssSelector(".flag")).Text;
                    DevStatsTxt.Text += "\nCountry: " + country;
                    string hand= productElement.FindElement(By.CssSelector(".pd_right li:nth-child(3) span:nth-child(2)")).Text;
                    DevStatsTxt.Text += "\nHand: " + hand;

                    tenisPlayer.Add(new TenisPlayer() { UserId = userId, Name = name, Age = age, Country = country, Hand = hand });
                }
                catch (NoSuchElementException)
                {
                    DevStatsTxt.Text += "\nAlgunos elementos no se encontraron y se omitieron\n";
                }
            }

            // Cerrar Selenium
            driver.Quit();

            // Insertar o actualizar en MongoDB
            var playersCollection = database.GetCollection<TenisPlayer>("tenis_players");
            foreach (var player in tenisPlayer)
            {
                var filter = Builders<TenisPlayer>.Filter.Eq(p => p.UserId, player.UserId);
                var optionss = new ReplaceOptions { IsUpsert = true };
                
                playersCollection.ReplaceOne(filter, player, optionss);
            }
            DevStatsTxt.Text += "\nProductos insertados o actualizados correctamente en MongoDB";
        }

        private void btnUpdMatch_Click(object sender, EventArgs e)
        {
            // Creating the connection to the MongoDB database
            MongoDBConnection dbConnection = new MongoDBConnection();
            var database = dbConnection.GetDatabase();

            // Configuración de Selenium con Chrome
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless"); // Opcional: ejecuta en modo sin interfaz gráfica
            var chromeService = ChromeDriverService.CreateDefaultService();
            chromeService.HideCommandPromptWindow = true; // Oculta la ventana de la consola

            ChromeDriver driver = new ChromeDriver(chromeService, options);

            // Navegar a la página
            driver.Navigate().GoToUrl("https://www.atptour.com/es/players?matchType=Singles&rank=Top%2010&region=all");

            // Esperar a que los productos carguen
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Creating the list that will keep the scraped data 
            var tenisPlayer = new List<TenisPlayer>();
            var tenisPlayerStats = new List<TenisPlayerStats>();
            var tenisPlayerActivity = new List<TenisPlayerActivity>();
            // Generamos una lista para guardar los urls de los tenistas
            List<string> playerUrls = [];
            List<string> ActivUrls = [];
            List<string> StatsUrls = [];

            // Creamos el log
            DevStatsTxt.Text = "";

            CargarCookies( driver, wait );

            // Obtener elementos de productos con Selenium
            wait.Until(d => d.FindElements(By.CssSelector(".player_cards .atp_card .card-link")));
            var productElements = driver.FindElements(By.CssSelector(".player_cards .atp_card .card-link"));

            // Iterating over the list of product HTML elements 
            foreach (var productElement in productElements)
            {
                // Obtiene el valor del atributo href
                playerUrls.Add(productElement.GetAttribute("href"));
            }

            // Cerrar Selenium
            driver.Quit();

            // Interactuamos con cada perfil del tenista
            foreach (var playerUrl in playerUrls)
            {
                DevStatsTxt.Text = "";
                driver = new ChromeDriver(chromeService, options);

                // Navegar a la página
                driver.Navigate().GoToUrl(playerUrl);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                CargarCookies(driver, wait);

                // Obtenemos los enlaces de la actividad y las estadisticas del tenista
                ActivUrls.Add(driver.FindElement(By.CssSelector(".tab-nav li:nth-child(3) a")).GetAttribute("href"));
                StatsUrls.Add(driver.FindElement(By.CssSelector(".tab-nav li:nth-child(4) a")).GetAttribute("href"));

                // Obtener elementos de productos con Selenium
                wait.Until(d => d.FindElements(By.CssSelector(".personal_details")));
                productElements = driver.FindElements(By.CssSelector(".personal_details"));

                // Iterating over the list of product HTML elements 
                foreach (var productElement in productElements)
                {
                    try
                    {
                        // De la Url actual Divide la URL y extraer la parte entre el nombre y "overview"
                        string[] partsUrl = driver.Url.Split('/');
                        string playerId = partsUrl[partsUrl.Length - 2];

                        string userId = playerId;
                        DevStatsTxt.Text += "\nId: " + userId;
                        string name = driver.FindElement(By.CssSelector(".player_name")).Text;
                        DevStatsTxt.Text += "\nName: " + name;
                        string age = productElement.FindElement(By.CssSelector(".pd_left li:first-child span:nth-child(2)")).Text.Split(' ')[0];
                        DevStatsTxt.Text += "\nAge: " + age;
                        string country = productElement.FindElement(By.CssSelector(".flag")).Text;
                        DevStatsTxt.Text += "\nCountry: " + country;
                        string hand = productElement.FindElement(By.CssSelector(".pd_right li:nth-child(3) span:nth-child(2)")).Text;
                        DevStatsTxt.Text += "\nHand: " + hand;

                        tenisPlayer.Add(new TenisPlayer() { UserId = userId, Name = name, Age = age, Country = country, Hand = hand });
                    }
                    catch (NoSuchElementException)
                    {
                        DevStatsTxt.Text += "\nAlgunos elementos no se encontraron y se omitieron\n";
                    }
                }

                driver.Quit();

                // Insertar o actualizar en MongoDB
                var playersCollection = database.GetCollection<TenisPlayer>("tenis_players");
                foreach (var player in tenisPlayer)
                {
                    var filter = Builders<TenisPlayer>.Filter.Eq(p => p.UserId, player.UserId);
                    var optionss = new ReplaceOptions { IsUpsert = true };

                    playersCollection.ReplaceOne(filter, player, optionss);
                }
                DevStatsTxt.Text += "\nProductos insertados o actualizados correctamente en MongoDB";
            }

            // Interactuamos con cada perfil stat del tenista
            foreach (var statsUrl in StatsUrls)
            {
                DevStatsTxt.Text = "";
                driver = new ChromeDriver(chromeService, options);

                // Navegar a la página
                driver.Navigate().GoToUrl(statsUrl);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                CargarCookies(driver, wait);

                // Obtener elementos de productos con Selenium
                wait.Until(d => d.FindElements(By.CssSelector(".statistics_content")));
                productElements = driver.FindElements(By.CssSelector(".statistics_content"));

                // Iterating over the list of product HTML elements 
                foreach (var productElement in productElements)
                {
                    try
                    {
                        // De la Url actual Divide la URL y extraer la parte entre el nombre y "overview"
                        string[] partsUrl = driver.Url.Split('/');
                        string playerId = partsUrl[partsUrl.Length - 2];

                        string[] wl = driver.FindElement(By.CssSelector(".stats-content .player-stats-details:nth-child(2) .wins")).Text.Split('-');

                        string userId = playerId;
                        string year = "all";
                        string win = wl[0];
                        string lose = wl[1];
                        string aces = productElement.FindElement(By.CssSelector("ul li:nth-child(1) .stats_percentage")).Text;
                        string doubFaul = productElement.FindElement(By.CssSelector("ul li:nth-child(2) .stats_percentage")).Text;
                        string fSer = productElement.FindElement(By.CssSelector("ul li:nth-child(3) .stats_percentage")).Text;
                        string fSerPoW = productElement.FindElement(By.CssSelector("ul li:nth-child(4) .stats_percentage")).Text;
                        string sSerPoW = productElement.FindElement(By.CssSelector("ul li:nth-child(5) .stats_percentage")).Text;
                        string breakPoF = productElement.FindElement(By.CssSelector("ul li:nth-child(6) .stats_percentage")).Text;
                        string breakPoS = productElement.FindElement(By.CssSelector("ul li:nth-child(7) .stats_percentage")).Text;
                        string serviGamesP = productElement.FindElement(By.CssSelector("ul li:nth-child(8) .stats_percentage")).Text;
                        string serviGamesW = productElement.FindElement(By.CssSelector("ul li:nth-child(9) .stats_percentage")).Text;
                        string totServiPoW = productElement.FindElement(By.CssSelector("ul li:nth-child(10) .stats_percentage")).Text;
                        string fSerRetPoW = productElement.FindElement(By.CssSelector("div:nth-of-type(2) ul li:nth-child(1) .stats_percentage")).Text;
                        string sSerRetPoW = productElement.FindElement(By.CssSelector("div:nth-of-type(2) ul li:nth-child(2) .stats_percentage")).Text;
                        string breakPoO = productElement.FindElement(By.CssSelector("div:nth-of-type(2) ul li:nth-child(3) .stats_percentage")).Text;
                        string breakPoC = productElement.FindElement(By.CssSelector("div:nth-of-type(2) ul li:nth-child(4) .stats_percentage")).Text;
                        string retGamesP = productElement.FindElement(By.CssSelector("div:nth-of-type(2) ul li:nth-child(5) .stats_percentage")).Text;
                        string retGamesW = productElement.FindElement(By.CssSelector("div:nth-of-type(2) ul li:nth-child(6) .stats_percentage")).Text;
                        string retPoW = productElement.FindElement(By.CssSelector("div:nth-of-type(2) ul li:nth-child(7) .stats_percentage")).Text;
                        string totPoW = productElement.FindElement(By.CssSelector("ul li:nth-child(8) .stats_percentage")).Text;

                        tenisPlayerStats.Add(new TenisPlayerStats() {   
                            UserId = userId, 
                            Year = year,
                            Win = win,
                            Lose = lose,
                            Aces = aces, 
                            DoubFaul = doubFaul, 
                            FSer = fSer, 
                            FSerPoW = fSerPoW,
                            SSerPoW = sSerPoW,
                            BreakPoF = breakPoF,
                            BreakPoS = breakPoS,
                            ServiGamesP = serviGamesP,
                            ServiGamesW = serviGamesW,
                            TotServiPoW = totServiPoW,
                            FSerRetPoW = fSerRetPoW,
                            SSerRetPoW = sSerRetPoW,
                            BreakPoO = breakPoO,
                            BreakPoC = breakPoC,
                            RetGamesP = retGamesP,
                            RetGamesW = retGamesW,
                            RetPoW = retPoW,
                            TotPoW = totPoW,
                        });
                    }
                    catch (NoSuchElementException)
                    {
                        DevStatsTxt.Text += "\nAlgunos elementos no se encontraron y se omitieron\n";
                    }
                }

                driver.Quit();

                // Insertar o actualizar en MongoDB
                var playersCollection = database.GetCollection<TenisPlayerStats>("tenis_player_stats");
                foreach (var tenisPlayerStat in tenisPlayerStats)
                {
                    var filter = Builders<TenisPlayerStats>.Filter.Eq(p => p.UserId, tenisPlayerStat.UserId);
                    var optionss = new ReplaceOptions { IsUpsert = true };

                    playersCollection.ReplaceOne(filter, tenisPlayerStat, optionss);
                }
                DevStatsTxt.Text += "\nProductos insertados o actualizados correctamente en MongoDB";
            }

            // Interactuamos con cada perfil activity del tenista
            foreach (var ActivUrl in ActivUrls)
            {
                DevStatsTxt.Text = "";
                driver = new ChromeDriver(chromeService, options);

                // Navegar a la página
                driver.Navigate().GoToUrl(ActivUrl);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                CargarCookies(driver, wait);

                // Obtener elementos de productos con Selenium
                wait.Until(d => d.FindElements(By.CssSelector(".tournament")));
                productElements = driver.FindElements(By.CssSelector(".tournament"));

                string[] partsUrl = driver.Url.Split('/');
                string playerId = partsUrl[partsUrl.Length - 2];

                // Iterating over the list of product HTML elements 
                foreach (var productElement in productElements)
                {
                    // Obtener elementos de productos con Selenium
                    wait.Until(d => d.FindElements(By.CssSelector(".ranking-item")));
                    var matchElements = productElement.FindElements(By.CssSelector(".ranking-item"));
                    var tournament = productElement.FindElement(By.CssSelector(".status-country .title")).Text;

                    foreach ( var matchElement in matchElements)
                    {
                        try
                        {
                            string[] matchUrl = matchElement.FindElement(By.CssSelector(".set-points a")).GetAttribute("href").Split('/');

                            string matchId = matchUrl[9];
                            string matchUser1 = playerId;
                            string matchUser2 = matchElement.FindElement(By.CssSelector(".name a")).GetAttribute("href").Split('/')[6];
                            string rd = matchElement.FindElement(By.CssSelector("dt")).Text;

                            string winner = "";
                            if (matchElement.FindElement(By.CssSelector(".set-check div span")).GetAttribute("class").Contains("icon-checkmark"))
                                winner = playerId;
                            else
                                winner = matchUser2;

                            string year = matchUrl[7];
                            string set1 = matchElement.FindElement(By.CssSelector(".set-points a div:nth-child(1)")).Text;
                            string set2 = matchElement.FindElement(By.CssSelector(".set-points a div:nth-child(2)")).Text;
                            string set3 = matchElement.FindElement(By.CssSelector(".set-points a div:nth-child(3)")).Text;
                            string set4 = matchElement.FindElement(By.CssSelector(".set-points a div:nth-child(4)")).Text;
                            string set5 = matchElement.FindElement(By.CssSelector(".set-points a div:nth-child(5)")).Text;

                            tenisPlayerActivity.Add(new TenisPlayerActivity()
                            {
                                MatchId = matchId,
                                MatchUser1 = matchUser1,
                                MatchUser2 = matchUser2,
                                Tournament = tournament,
                                Rd = rd,
                                Winner = winner,
                                Year = year,
                                Set1 = set1,
                                Set2 = set2,
                                Set3 = set3,
                                Set4 = set4,
                                Set5 = set5,
                            });
                        }
                        catch (NoSuchElementException)
                        {
                            DevStatsTxt.Text += "\nAlgunos elementos no se encontraron y se omitieron\n";
                        }
                    }
                }

                driver.Quit();

                // Insertar o actualizar en MongoDB
                var playersCollection = database.GetCollection<TenisPlayerActivity>("tenis_player_activity");
                foreach (var tenisPlayerActivit in tenisPlayerActivity)
                {
                    var filter = Builders<TenisPlayerActivity>.Filter.Eq(p => p.MatchId, tenisPlayerActivit.MatchId);
                    var optionss = new ReplaceOptions { IsUpsert = true };

                    playersCollection.ReplaceOne(filter, tenisPlayerActivit, optionss);
                }
                DevStatsTxt.Text += "\nProductos insertados o actualizados correctamente en MongoDB";
            }
        }

        private void btnUpdStats_Click(object sender, EventArgs e)
        {
            
        }

        private void CargarCookies(ChromeDriver driver, WebDriverWait wait)
        {
            try
            {
                wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString() == "complete");
                DevStatsTxt.Text += "Carga Inicial SI completada\n";
            }
            catch (WebDriverTimeoutException)
            {
                DevStatsTxt.Text += "Carga Inicial NO completada\n";
            }

            // Comprobar si las cookies están presentes y hacer clic si es necesario
            int i = 0;
            while (i <= 5)
            {
                Thread.Sleep(500);
                var acceptButton = driver.FindElements(By.Id("onetrust-accept-btn-handler")).FirstOrDefault();
                if (acceptButton != null)
                {
                    acceptButton.Click(); // Hacer clic si el botón está presente
                    DevStatsTxt.Text += "Cookies SI encontradas\n";
                    break;
                }
                else
                    DevStatsTxt.Text += "Cookies NO encontradas\n";
                i++;
            }
        }
    }
}
