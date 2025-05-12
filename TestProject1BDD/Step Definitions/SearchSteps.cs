using Reqnroll;
using TestProject1.Core.Drivers;
using Tests.Pages;

namespace TestProject1BDD.Step_Definitions
{
    [Binding]
    public class SearchSteps
    {
        private readonly SearchPage? _searchPage;

        public SearchSteps()
        {
            this._searchPage = new SearchPage(WebDriverSingleton.Driver);
        }

        //Career search

        [Given(@"I am on the EPAM career search page")]
        public void GivenIAmOnTheCareerSearchPage()
        {
            this._searchPage?.NavigateToCareerSearch();
        }

        [When(@"I search for ""(.*)"" remote careers in all locations")]
        public void WhenISearchFor(string language)
        {
            this._searchPage?.EnterCareerSearchDetails(language);
        }

        [Then(@"I should see ""(.*)"" in the search results")]
        public void ThenIShouldSeeInResults(string language)
        {
            this._searchPage?.ValidateProgramingLanguageIsOnPage(language);
        }

        //Global search

        [Given(@"I am on the EPAM global search")]
        public void GivenIAmOnTheHomePage()
        {
            this._searchPage?.StartGlobalSearch();
        }

        [When(@"I perform a global search for ""(.*)""")]
        public void WhenIPerformSearch(string searchOption)
        {
            this._searchPage?.EnterTextInSearchForm(searchOption);
        }

        [Then(@"all the result links should contain ""(.*)""")]
        public void ThenAllResultLinksShouldContain(string searchOption)
        {
            this._searchPage?.ValidateAllLinksContainText(searchOption);
        }
    }
}
