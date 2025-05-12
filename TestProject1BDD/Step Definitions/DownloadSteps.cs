using Reqnroll;
using TestProject1.Core.Drivers;
using Tests.Pages;

namespace TestProject1BDD.Step_Definitions
{
    [Binding]
    public class DownloadSteps
    {
        private readonly AboutPage? _aboutPage;
        private readonly string? _downloadsPath;

        public DownloadSteps()
        {
            this._aboutPage = new AboutPage(WebDriverSingleton.Driver);
            this._downloadsPath = WebDriverSingleton.DownloadsPath;
        }

        [Given(@"I am on the EPAM About page")]
        public void GivenIAmOnTheAboutPage()
        {
            this._aboutPage?.NavigateToAbout();
        }

        [When(@"I download the corporate overview file")]
        public void WhenIDownloadTheFile()
        {
            this._aboutPage?.StartDownloading();
        }

        [Then(@"the file ""(.*)"" should be downloaded")]
        public void ThenTheFileShouldBeDownloaded(string expectedFileName)
        {
            string fullPath = Path.Combine(this._downloadsPath ?? string.Empty, expectedFileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            this._aboutPage?.ValidateTheFileHasBeenDownloaded(fullPath, expectedFileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
