using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;
using Tests.Pages;

namespace TestProject1BDD.Business.Page_Objects
{
    public class ServicePage : BasePage
    {
        public ServicePage(IWebDriver? driver) : base(driver)
        {
        }

        //Locators

        internal IWebElement ServiceLink => base.FindElementByXPath("//a[contains(@href, 'services') and contains(@class, 'top-navigation__item-link')]");
        
        internal ReadOnlyCollection<IWebElement> Spans => base.FindElementsByXPath("//div[@class='section']//div[@class='text']//span");

        // Public methods

        public void HoverOverServiceLink()
        {
            Actions actions = new Actions(base.Driver 
                ?? throw new NullReferenceException("Web driver has not been created"));

            actions.MoveToElement(this.ServiceLink).Perform();
        }

        public void ClickOnParticularServiceLink(string? particularService)
        {
            if (string.IsNullOrWhiteSpace(particularService))
                throw new ArgumentException("Service name must be provided");

            var linkText = new string(particularService
                .Select(ch => ch.Equals(' ') ? '-' : char.ToLower(ch))
                .ToArray());
            
            var element = base.FindElementByXPath($"//a[@href='/services/artificial-intelligence/{linkText}' and contains(@class, 'top-navigation__sub-link')]");

            element.Click();
        }

        //Validating methods

        public void ValiadteThePageContainsCorrectTitle(string? correctTitle)
        {
            //Act
            bool containsTitle = this.Spans.Any(e => e.Text.Trim().Equals(correctTitle, StringComparison.OrdinalIgnoreCase));

            //Assert
            Assert.IsTrue(containsTitle, $"Page should contain '{correctTitle}'");
        }

        public void ValidateOurRelatedExpertiseIsDisplayed()
        {
            //Validate the secion is on the page
            //Act
            var section = this.Spans.FirstOrDefault(e => e.Text.Trim().Equals("Our Related Expertise", StringComparison.OrdinalIgnoreCase));

            //Assert
            Assert.IsTrue(section is not null, "Page should contain 'Our Related Expertise' section");

            //Validate the section is displayed
            //Act
            base.ExecuteScrolling(section);

            //Assert
            Assert.IsTrue(section?.Displayed, "Page should display 'Our Related Expertise' section");
        }
    }
}
