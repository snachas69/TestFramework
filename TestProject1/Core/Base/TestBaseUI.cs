using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Serilog;
using TestProject1.Core.Drivers;

namespace TestProject1.Core.Base
{
    public abstract class TestBaseUI : TestBase
    {
        protected IWebDriver? Driver;

        protected string? DownloadsPath;

        [OneTimeSetUp]
        public void WebSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Information("Configuration variable has been set successfully");

            WebDriverSingleton.BrowserType = config["Browser"] ?? "Chrome";

            this.DownloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            WebDriverSingleton.DownloadsPath = this.DownloadsPath;

            this.Driver = WebDriverSingleton.Driver;

            this.Driver.Manage().Window.Maximize();

            this.Driver.Url = "https://www.epam.com/";
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            WebDriverSingleton.Dispose();
        }
    }
}
