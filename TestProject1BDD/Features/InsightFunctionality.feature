Feature: EPAM Insights Page Validation
  As a site visitor
  I want to ensure the article title matches the carousel title
  So that I can trust the content structure on the EPAM insights page

  Scenario: Verify article title matches carousel title
    Given I am on the EPAM insights page
    Then the article title should match the carousel title