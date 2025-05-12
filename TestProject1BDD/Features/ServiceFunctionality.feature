Feature: Validate Navigation to Services Section
  As a user
  I want to navigate through the EPAM Services menu
  So that I can view a specific AI service and verify its content

  Scenario Outline: Validate navigation to a specific AI service section
    Given I am on EPAM home bage hovering over the "Services" link
    When I click on the "<ServiceName>" category under Services 
    Then the page title should be "<ServiceName>"
    And the "Our Related Expertise" section should be displayed

    Examples:
      | ServiceName     |
      | Generative AI   |
      | Responsible AI  |
