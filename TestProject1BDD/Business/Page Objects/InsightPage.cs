using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tests.Pages
{
    public class InsightPage : BasePage
    {
        public InsightPage(IWebDriver? driver) : base(driver)
        {
        }

        //Locators

        internal IWebElement InsightLink => base.FindElementByXPath("//a[contains(@href, 'insights') and contains(@class, 'top-navigation__item-link')]");

        internal IWebElement NextCarouselButton => base.FindElementByXPath("//div[contains(@class, 'slider__navigation')]/button[contains(@class, 'slider__right-arrow')]");

        internal IWebElement ReadMoreButton => base.FindElementByXPath("//div[contains(@class, 'active')]//div[@data-link-name='Read More']/a");

        internal IWebElement SlideTitle => base.FindElementByXPath("//div[contains(@class, 'active')]//div[@class='single-slide-ui']//p");

        internal IWebElement ArticleTitle => base.FindElementByXPath("//div[@class='header_and_download']//span");

        //Public Methods

        public void NavigateToInsightPage() => this.InsightLink.Click();

        public void SwipeCarouselTimes(int times)
        {
            base.ExecuteScrolling(this.NextCarouselButton);

            for (int i = 0; i < times; i++)
            {
                this.NextCarouselButton.Click();
            }
        }

        public bool CompareTitles()
        {
            var wait = new WebDriverWait(this.Driver ?? throw new NullReferenceException("Web driver has not been created"),
                TimeSpan.FromSeconds(20));

            wait.Until(driver =>
            {
                try
                {
                    return this.ReadMoreButton.Displayed && this.ReadMoreButton.Enabled;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            string slideTitle = this.SlideTitle.Text.Trim();

            this.ReadMoreButton.Click();

            string articleTitle = this.ArticleTitle.Text.Trim();

            return slideTitle.Equals(articleTitle);
        }

        //Validating methods

        public void ValidateTitleOfArticleMatchesTitleOfCarousel()
        {
            //Act
            if (!AreCookiesAccepted)
            {
                base.WaitForAcceptingCookies();
            }

            this.SwipeCarouselTimes(2);

            bool result = this.CompareTitles();

            //Assert
            Assert.That(result, Is.True, "The article title does not match the carousel title.");
        }
    }
}
