using OpenQA.Selenium;
using Serilog;

namespace Tests.Pages
{
    public class AboutPage : BasePage
    {
        public AboutPage(IWebDriver? driver) : base(driver)
        {
        }

        //Locators

        internal IWebElement AboutLink => FindElementByXPath("//a[contains(@href, 'about') and contains(@class, 'top-navigation__item-link')]");

        internal IWebElement Download => FindElementByXPath("//a[@download]");

        //Public Methods

        public void StartDownloading()
        {
            Log.Information("[AboutPage] Navigating to About page and initiating download.");

            this.AboutLink.Click();

            Log.Information("[AboutPage] Clicked About link.");

            this.WaitForAcceptingCookies();

            Log.Information("[AboutPage] Checked for cookie acceptance.");

            this.ExecuteScrolling(this.Download);

            Log.Information("[AboutPage] Scrolled to download link.");

            this.Download.Click();

            Log.Information("[AboutPage] Clicked download link.");
        }

        public bool WaitUntilFileExists(string filePath, TimeSpan timeout)
        {
            Log.Information($"[AboutPage] Waiting for file to exist: {filePath}");

            var waitUntil = DateTime.Now + timeout;

            while (DateTime.Now < waitUntil)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }

                Log.Information("[AboutPage] Timeout reached. File not found.");

                Thread.Sleep(500);
            }

            return false;
        }

        //Validating Methods

        public void ValidateTheFileHasBeenDownloaded(string fullFilePath, string expectedFileName)
        {
            Log.Information($"[AboutPage] Validating download of file: {expectedFileName}");

            //Act
            bool isDownloaded = this.WaitUntilFileExists(fullFilePath, TimeSpan.FromSeconds(30));

            Log.Information(isDownloaded
                ? $"[AboutPage] File '{expectedFileName}' downloaded successfully."
                : $"[AboutPage] File '{expectedFileName}' was NOT downloaded in time.");

            //Assert
            Assert.That(isDownloaded, Is.True, $"File \'{expectedFileName}\' was not downloaded within the expected time.");
        }
    }
}
