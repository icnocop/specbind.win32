Feature: Launch Application

Scenario: Launch application
    Given I launched the Win32 application
    Then the Main window is displayed

Scenario: Attach to application
    Given I launched the Win32 application
    And I attached to the Win32 application

Scenario: Launch application and check exit code
    Given I launched the Win32 application
    And the Main window was displayed
    When I click OK
    Then the Main window will close
    And the Win32 application will exit
    And the Win32 application will exit with code 0

Scenario: Wait for window to display
    Given I launched the Win32 application
    And I waited for the Main window to be displayed

Scenario: Wait for child window to display
    Given I launched the Win32 application
    And the Main window was displayed
    When I click Display child dialog
    And I wait for the Child window to display

@Highlight
Scenario: Wait for window to close
    Given I launched the Win32 application
    And the Main window was displayed
    And I clicked Display child dialog
    And the Child window was displayed
    # This next step fails
    And I clicked OK
    When I wait for the Child window to close
    Then the Child window will close