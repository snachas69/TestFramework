Feature: Search
  The search functionality allows users to find relevant information
  either through the career search or the global site search.

  Scenario Outline: Career search by programming language
    Given I am on the EPAM career search page
    When I search for "<Language>" remote careers in all locations
    Then I should see "<Language>" in the search results

    Examples: 
      | Language   |
      | Java       |
      | C#         |
      | JavaScript |

  Scenario Outline: Global search returns relevant links
    Given I am on the EPAM global search
    When I perform a global search for "<SearchOption>"
    Then all the result links should contain "<SearchOption>"

    Examples:
      | SearchOption |
      | Blockchain   |
      | Cloud        |
      | Automation   |