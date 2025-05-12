Feature: Download Corporate Overview File
  As a user on the EPAM About page
  I want to download the corporate overview file
  So that I can access the company's key information offline

  Scenario: Verify corporate overview file is downloaded
    Given I am on the EPAM About page
    When I download the corporate overview file
    Then the file "EPAM_Corporate_Overview_Q4FY-2024.pdf" should be downloaded
