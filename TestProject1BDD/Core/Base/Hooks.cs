using Microsoft.Extensions.Configuration;
using Reqnroll;
using TestProject1.Core.Drivers;

namespace TestProject1.Core.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        [BeforeTestRun]
        public static void BeforeScenario()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            WebDriverSingleton.BrowserType = config["Browser"] ?? "Chrome";

            string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            WebDriverSingleton.DownloadsPath = downloadsPath;

            var driver = WebDriverSingleton.Driver;

            driver.Manage().Window.Maximize();

            driver.Url = "https://www.epam.com/";
        }

        [AfterTestRun]
        public static void AfterScenario()
        {
            WebDriverSingleton.Dispose();
        }
    }
}
