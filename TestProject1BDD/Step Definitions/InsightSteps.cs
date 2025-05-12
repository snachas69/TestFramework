using Reqnroll;
using TestProject1.Core.Drivers;
using Tests.Pages;

namespace TestProject1BDD.Step_Definitions
{
    [Binding]
    public class InsightSteps
    {
        private readonly InsightPage? _insightPage;

        public InsightSteps()
        {
            this._insightPage = new InsightPage(WebDriverSingleton.Driver);
        }

        [Given(@"I am on the EPAM insights page")]
        public void GivenIAmOnTheInsightsPage()
        {
            this._insightPage?.NavigateToInsightPage();
        }

        [Then(@"the article title should match the carousel title")]
        public void ThenTitleShouldMatch()
        {
            this._insightPage?.ValidateTitleOfArticleMatchesTitleOfCarousel();
        }

    }
}
