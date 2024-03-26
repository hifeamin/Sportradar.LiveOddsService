Feature: Match

Scenario: Start new match in empty list
	When start new match for home team "h1" and away team "a1"
	Then the result should not be null
	And the result home team should be "h1"
	And the result away team should be "a1"

Scenario: Match should exist in summery and check start date
	Given start new match for home team "h1" and away team "a1"
	And wait for 2 second
	When get match summery
	Then the summery should have match with home team "h1" and away team "a1" and consider result
	And start date of result should be older than 2 second ago