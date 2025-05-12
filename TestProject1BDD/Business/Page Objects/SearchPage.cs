using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tests.Pages
{
    public class SearchPage : BasePage
    {
        public SearchPage(IWebDriver? driver) : base(driver)
        {
        }

        // Locators
        internal IWebElement CareerLink => base.FindElementByXPath("//a[contains(@href, 'careers') and contains(@class, 'top-navigation__item-link')]");

        internal IWebElement KeyWordField => base.FindElementById("new_form_job_search-keyword");

        internal IWebElement RemoteCheckbox => base.FindElementByXPath("//input[@type='checkbox' and @name='remote']/following-sibling::label");

        internal IWebElement FindButton => base.FindElementByCss("form#jobSearchFilterForm button[type='submit']");

        internal IWebElement LocationField => base.FindElementByXPath("//select[@id='new_form_job_search-location']/parent::*");

        internal IWebElement LatestElementViewAndApplyButton => base.FindElementByXPath("//ul[@class='search-result__list']/li[last()]//a[contains(@class, 'item-apply')]");

        internal IWebElement VacancyDetails => base.FindElementByXPath("//section[contains(@class, 'vacancy-details-23')]//div[contains(@class, 'content')]");

        internal IWebElement SearchIcon => base.FindElementByXPath("//button[contains(@class, 'header-search__button')]");

        internal IWebElement SearchForm => base.FindElementByXPath("//form[contains(@class, 'header-search')]//div//div//input");

        internal IWebElement SearchButton => base.FindElementByXPath("//form[contains(@class, 'header-search')]//button[contains(@class, 'search-button')]");

        internal IWebElement SearchResult => base.FindElementByXPath("//div[@class = 'search-results__items']");

        //Public methods
        public IWebElement SelectedOption(string text) => base.FindElementByXPath($"//li[normalize-space(text())='{text}']");

        public void NavigateToCareerSearch() => this.CareerLink.Click();

        public void StartGlobalSearch() => this.SearchIcon.Click();

        public void EnterCareerSearchDetails(string language)
        {
            base.ExecuteScrolling(SearchForm);

            this.KeyWordField.SendKeys(language);

            this.SelectLocationByText("All Locations");

            this.RemoteCheckbox.Click();

            this.FindButton.Click();
        }

        public void SelectLocationByText(string text)
        {
            this.LocationField.Click();

            this.SelectedOption(text).Click();
        }

        public void EnterTextInSearchForm(string text)
        {
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

            this.SearchForm.SendKeys(text);

            this.SearchButton.Click();
        }

        //Validating methods

        public void ValidateProgramingLanguageIsOnPage(string language)
        {
            //Act
            base.ExecuteScrolling(this.LatestElementViewAndApplyButton);

            this.LatestElementViewAndApplyButton.Click();

            bool contains = this.VacancyDetails.Text.IndexOf(language, StringComparison.OrdinalIgnoreCase) >= 0;

            //Assert
            Assert.That(contains, Is.True, $"The language {language} was not found on the page.");
        }

        public void ValidateAllLinksContainText(string text)
        {
            //Arrange
            var articles = base.FindElementsByXPath(".//article");

            //Act
            var allArticlesContainKeyword = articles.All(article =>
            {
                var articleText = article.Text.Trim();

                return articleText.Contains(text, StringComparison.OrdinalIgnoreCase);
            });

            // Assert that all articles contain the keywords
            Assert.That(allArticlesContainKeyword, Is.True, "Not all articles contain the required keywords.");
        }
    }
}
