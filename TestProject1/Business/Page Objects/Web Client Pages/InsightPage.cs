using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;

namespace Tests.Pages
{
    public class InsightPage : BasePage
    {
        public InsightPage(IWebDriver? driver) : base(driver)
        {
        }

        //Locators

        internal IWebElement InsightLink => FindElementByXPath("//a[contains(@href, 'insights') and contains(@class, 'top-navigation__item-link')]");

        internal IWebElement NextCarouselButton => FindElementByXPath("//div[contains(@class, 'slider__navigation')]/button[contains(@class, 'slider__right-arrow')]");

        internal IWebElement ReadMoreButton => FindElementByXPath("//div[contains(@class, 'active')]//div[@data-link-name='Read More']/a");

        internal IWebElement SlideTitle => FindElementByXPath("//div[contains(@class, 'active')]//div[@class='single-slide-ui']//p");

        internal IWebElement ArticleTitle => FindElementByXPath("//div[contains(@class, 'section__wrapper')]/div[contains(@class, 'column-control')]//div[@class='text']/div/p");

        //Public Methods

        public void SwipeCarouselTimes(int times)
        {
            this.InsightLink.Click();

            this.ExecuteScrolling(this.NextCarouselButton);

            for (int i = 0; i < times; i++)
            {
                Log.Information($"[InsightPage] Clicking next on carousel: {i + 1}/{times}");

                this.NextCarouselButton.Click();
            }
        }

        public bool CompareTitles()
        {
            var wait = new WebDriverWait(this.Driver ?? throw new NullReferenceException("Web driver has not been created"), 
                TimeSpan.FromSeconds(5));
            
            wait.Until(driver =>
            {
                try
                {
                    return this.ReadMoreButton.Displayed && this.ReadMoreButton.Enabled;
                }
                catch (StaleElementReferenceException)
                {
                    Log.Information("[InsightPage] Caught StaleElementReferenceException, retrying...");
                    return false;
                }
            });

            string slideTitle = this.SlideTitle.Text.Trim();

            Log.Information($"[InsightPage] Slide title captured: \"{slideTitle}\"");

            this.ReadMoreButton.Click();

            string articleTitle = this.ArticleTitle.Text.Trim();

            Log.Information($"[InsightPage] Article title captured: \"{articleTitle}\"");

            return slideTitle.Equals(articleTitle);
        }

        //Validate
        
        public void ValidateTitleOfArticleMatchesTitleOfCarousel()
        {
            //Act
            this.SwipeCarouselTimes(2);

            bool result = this.CompareTitles();

            //Assert
            Assert.That(result, Is.True, "The article title does not match the carousel title.");
        }
    }
}
