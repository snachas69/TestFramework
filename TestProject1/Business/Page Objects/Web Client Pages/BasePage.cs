﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;

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
                TimeSpan.FromSeconds(30));

            Log.Information("Base page for UI testing has been created");
        }

        internal IWebElement AcceptCookieButton => this.FindElementById("onetrust-accept-btn-handler");

        private IWebElement WaitForElement(By by) => this._wait.Until(driver => driver.FindElement(by));

        protected IWebElement FindElementByCss(string selector) => this.WaitForElement(By.CssSelector(selector));

        protected IWebElement FindElementByXPath(string xpath) => this.WaitForElement(By.XPath(xpath));

        protected IWebElement FindElementById(string id) => this.WaitForElement(By.Id(id));

        protected IWebElement FindElementByClassName(string className) => this.WaitForElement(By.ClassName(className));

        public string? GetPageTitle()
        {
            return this.Driver?.Title ?? string.Empty;
        }

        public void WaitForAcceptingCookies()
        {
            var wait = new WebDriverWait(this.Driver ?? throw new ArgumentNullException("Web driver has not been created"), 
                TimeSpan.FromSeconds(20));
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
