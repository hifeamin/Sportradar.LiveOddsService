Feature: Match

Scenario: Start new match in empty list
	When Start new match for home team "h1" and away team "a1"
	Then the result should not be null
	And the result home team should be "h1"
	And the result away team should be "a1"
