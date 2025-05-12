using Reqnroll;
using TestProject1.Core.Drivers;
using TestProject1BDD.Business.Page_Objects;

namespace TestProject1BDD.Step_Definitions
{
    [Binding]
    public class ServiceSteps
    {
        private readonly ServicePage? _servicePage;

        public ServiceSteps()
        {
            this._servicePage = new ServicePage(WebDriverSingleton.Driver);
        }

        [Given(@"I am on EPAM home bage hovering over the ""Services"" link")]
        public void WhenIHoverOverTheLink()
        {
            this._servicePage?.HoverOverServiceLink();
        }

        [When(@"I click on the ""(.*)"" category under Services")]
        public void WhenIClickOnTheCategoryUnderServices(string serviceName)
        {
            this._servicePage?.ClickOnParticularServiceLink(serviceName);
        }

        [Then(@"the page title should be ""(.*)""")]
        public void ThenThePageTitleShouldBe(string serviceName)
        {
            this._servicePage?.ValiadteThePageContainsCorrectTitle(serviceName);
        }

        [Then(@"the ""Our Related Expertise"" section should be displayed")]
        public void ThenTheSectionShouldBeDisplayed()
        {
            this._servicePage?.ValidateOurRelatedExpertiseIsDisplayed();
        }
    }
}
