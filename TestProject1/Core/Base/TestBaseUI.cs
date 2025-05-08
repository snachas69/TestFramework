using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
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
        public void OneTimeTearDown()
        {
            WebDriverSingleton.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;

            if (outcome == TestStatus.Failed)
            {
                CaptureScreenshot(TestContext.CurrentContext.Test.Name);
            }
        }

        private void CaptureScreenshot(string testName)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var screenshotsDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestResults", "Screenshots");
                Directory.CreateDirectory(screenshotsDir);

                var filePath = Path.Combine(screenshotsDir, $"{testName}_{timestamp}.png");
                ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(filePath);

                TestContext.AddTestAttachment(filePath, "Screenshot on Failure");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Failed to save screenshot: {ex.Message}");
            }
        }
    }
}
