using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tests.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver? Driver;
        private WebDriverWait _wait;

        public BasePage(IWebDriver? driver)
        {
            this.Driver = driver;
            this._wait = new WebDriverWait(this.Driver ?? throw new ArgumentNullException("Web Driver has not been created"), 
                TimeSpan.FromSeconds(10));
        }

        internal IWebElement AcceptCookieButton => FindElementById("onetrust-accept-btn-handler");

        protected IWebElement WaitForElement(By by)
        {
            return this._wait.Until(driver => driver.FindElement(by));
        }

        protected IWebElement FindElementByCss(string selector) => WaitForElement(By.CssSelector(selector));

        protected IWebElement FindElementByXPath(string xpath) => WaitForElement(By.XPath(xpath));

        protected IWebElement FindElementById(string id) => WaitForElement(By.Id(id));

        protected IWebElement FindElementByClassName(string className) => WaitForElement(By.ClassName(className));

        public string? GetPageTitle()
        {
            return this.Driver?.Title ?? string.Empty;
        }

        public void WaitForAcceptingCookies()
        {
            var wait = new WebDriverWait(this.Driver ?? throw new ArgumentNullException("Web driver has not been created"), 
                TimeSpan.FromSeconds(5));
            var acceptButton = wait.Until(driver => this.AcceptCookieButton.Displayed ? this.AcceptCookieButton : null);

            acceptButton?.Click();
        }

        public void ExecuteScrolling(IWebElement? webElement)
        {
            if (this.Driver is null)
            {
                throw new ArgumentNullException("Web driver has not been created");
            }

            ((IJavaScriptExecutor)this.Driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'smooth', block:'center'});", webElement);
        }
    }
}
