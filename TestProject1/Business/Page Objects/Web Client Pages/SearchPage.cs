using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;

namespace Tests.Pages
{
    public class SearchPage : BasePage
    {
        public SearchPage(IWebDriver? driver) : base(driver)
        {
            Log.Information("Search page has been created");
        }

        // Locators
        internal IWebElement CareerLink => FindElementByXPath("//a[contains(@href, 'careers') and contains(@class, 'top-navigation__item-link')]");

        internal IWebElement KeyWordField => FindElementById("new_form_job_search-keyword");

        internal IWebElement RemoteCheckbox => FindElementByXPath("//input[@type='checkbox' and @name='remote']/following-sibling::label");

        internal IWebElement FindButton => FindElementByCss("form#jobSearchFilterForm button[type='submit']");

        internal IWebElement LocationField => FindElementByXPath("//select[@id='new_form_job_search-location']/parent::*");

        internal IWebElement LatestElementViewAndApplyButton => FindElementByXPath("//ul[@class='search-result__list']/li[last()]//a[contains(@class, 'item-apply')]");

        internal IWebElement VacancyDetails => FindElementByXPath("//section[contains(@class, 'vacancy-details-23')]//div[contains(@class, 'content')]");

        internal IWebElement SearchIcon => FindElementByXPath("//button[contains(@class, 'header-search__button')]");

        internal IWebElement SearchForm => FindElementByXPath("//form[contains(@class, 'header-search')]//div//div//input");
        
        internal IWebElement SearchButton => FindElementByXPath("//form[contains(@class, 'header-search')]//button[contains(@class, 'search-button')]");

        internal IWebElement SearchResult => FindElementByXPath("//div[@class = 'search-results__items']");

        //Public methods

        public void EnterCareerSearchDetails(string language)
        {
            Log.Information($"[SearchPage] Starting career search with keyword: {language}");

            this.CareerLink.Click();

            Log.Information("[SearchPage] Clicked on Career link.");

            this.KeyWordField.SendKeys(language);

            Log.Information($"[SearchPage] Entered keyword: {language}");

            this.SelectLocationByText("All Locations");

            Log.Information("[SearchPage] Selected location: All Locations");

            this.RemoteCheckbox.Click();

            Log.Information("[SearchPage] Clicked on Remote checkbox.");

            this.FindButton.Click();

            Log.Information("[SearchPage] Clicked Find button.");
        }

        public void SelectLocationByText(string text)
        {
            Log.Information($"[SearchPage] Selecting location by text: {text}");

            this.LocationField.Click();

            SelectedOption(text).Click();

            Log.Information($"[SearchPage] Location '{text}' selected.");
        }

        public IWebElement SelectedOption(string text)
        {
            Log.Information($"[SearchPage] Finding location option: {text}");

            return FindElementByXPath($"//li[normalize-space(text())='{text}']");
        }

        public void EnterTextInSearchForm(string text)
        {
            Log.Information($"[SearchPage] Entering text in search form: {text}");

            this.SearchIcon.Click();

            Log.Information("[SearchPage] Clicked Search icon.");

            var wait = new WebDriverWait(base.Driver ?? throw new NullReferenceException("Web driver has not been created"), 
                TimeSpan.FromSeconds(30));

            wait.Until(driver =>
            {
                try
                {
                    return this.SearchForm.Displayed && this.SearchForm.Enabled;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            SearchForm.SendKeys(text);
            
            Log.Information("[SearchPage] Entered search text.");

            this.SearchButton.Click();

            Log.Information("[SearchPage] Clicked Search button.");
        }
        //Validating methods
        public void ValidateProgramingLanguageIsOnPage(string language)
        {
            Log.Information($"[SearchPage] Validating presence of language: {language}");

            //Act

            ExecuteScrolling(this.LatestElementViewAndApplyButton);

            this.LatestElementViewAndApplyButton.Click();

            Log.Information("[SearchPage] Clicked latest 'View and Apply' button.");

            bool contains = this.VacancyDetails.Text.IndexOf(language, StringComparison.OrdinalIgnoreCase) >= 0;

            Log.Information($"[SearchPage] Language {(contains ? "found" : "not found")} on the page.");

            //Assert
            Assert.That(contains, Is.True, $"The language {language} was not found on the page.");
        }

        public void ValidateAllLinksContainText(string text)
        {
            Log.Information($"[SearchPage] Validating all search results contain: {text}");

            //Arrange
            var articles = this.SearchResult.FindElements(By.XPath(".//article"));

            //Act
            var allArticlesContainKeyword = articles.All(article =>
            {
                var articleText = article.Text.Trim();

                return articleText.Contains(text, StringComparison.OrdinalIgnoreCase);
            });

            Log.Information(allArticlesContainKeyword
                ? "[SearchPage] All articles contain the expected text."
                : "[SearchPage] One or more articles do NOT contain the expected text.");

            // Assert that all articles contain the keywords
            Assert.That(allArticlesContainKeyword, Is.True, "Not all articles contain the required keywords.");
        }
    }
}
