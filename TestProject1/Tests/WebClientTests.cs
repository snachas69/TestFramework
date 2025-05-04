using TestProject1.Core.Base;
using Tests.Pages;

namespace TestProject1.Tests
{
    [TestFixture, Category("Web")]
    public class WebClientTests : TestBaseUI
    {
        private SearchPage _searchPage;
        private AboutPage _aboutPage;
        private InsightPage _insightPage;

        public static IEnumerable<string> Languages => new[] { "Java", "C#", "JavaScript" };

        public static IEnumerable<string> SearchOptions => new[] { "BLOCKCHAIN", "Cloud", "Automation" };

        [SetUp]
        public void Setup()
        {
            this._searchPage = new SearchPage(base.Driver);
            this._aboutPage = new AboutPage(base.Driver);
            this._insightPage = new InsightPage(base.Driver);
        }

        [Test, TestCaseSource(nameof(Languages))]
        public void Test_UserCanSearchForPositionBasedOnCriteria(string language)
        {
            this._searchPage.EnterCareerSearchDetails(language);

            this._searchPage.ValidateProgramingLanguageIsOnPage(language);
        }

        [Test, TestCaseSource(nameof(SearchOptions))]
        public void Test_GlobalSearchWorksCorrectly(string searchOption)
        {
            this._searchPage.EnterTextInSearchForm(searchOption);

            this._searchPage.ValidateAllLinksContainText(searchOption);
        }

        [Test]
        public void Test_FileDownloadWorksCorrectly()
        {
            this._aboutPage.StartDownloading();

            string expectedFileName = "EPAM_Corporate_Overview_Q4FY-2024.pdf";
            string fullFilePath = Path.Combine(DownloadsPath ?? string.Empty, expectedFileName);

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            this._aboutPage.ValidateTheFileHasBeenDownloaded(fullFilePath, expectedFileName);

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }
        }

        [Test]
        public void Test_TitleOfArticleMatchesTitleOfCarousel()
        {
            this._insightPage.ValidateTitleOfArticleMatchesTitleOfCarousel();
        }
    }
}