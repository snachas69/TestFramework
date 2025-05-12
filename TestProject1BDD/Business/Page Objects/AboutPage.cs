using OpenQA.Selenium;

namespace Tests.Pages
{
    public class AboutPage : BasePage
    {
        public AboutPage(IWebDriver? driver) : base(driver)
        {
        }

        //Locators

        internal IWebElement AboutLink => base.FindElementByXPath("//a[contains(@href, 'about') and contains(@class, 'top-navigation__item-link')]");

        internal IWebElement Download => base.FindElementByXPath("//a[@download]");

        //Public Methods

        public void NavigateToAbout() => this.AboutLink.Click();

        public void StartDownloading()
        {
            base.WaitForAcceptingCookies();

            base.ExecuteScrolling(this.Download);

            this.Download.Click();
        }

        public bool WaitUntilFileExists(string filePath, TimeSpan timeout)
        {
            var waitUntil = DateTime.Now + timeout;

            while (DateTime.Now < waitUntil)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }

                Thread.Sleep(500);
            }

            return false;
        }

        //Validating Methods

        public void ValidateTheFileHasBeenDownloaded(string fullFilePath, string expectedFileName)
        {
            //Act
            bool isDownloaded = this.WaitUntilFileExists(fullFilePath, TimeSpan.FromSeconds(30));

            //Assert
            Assert.That(isDownloaded, Is.True, $"File \'{expectedFileName}\' was not downloaded within the expected time.");
        }
    }
}
