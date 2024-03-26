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

Scenario: Update match should be updated in get summery
	Given start new match for home team "h1" and away team "t1"
	When update home team "h1" score to 2 and away team "t1" to 3
	And get match summery
	Then the summery should have match with home team "h1" and away team "t1" and consider result
	And the result should have home team score 2
	And the result should have away team score 3

Scenario: Get summery in order of total score and most recent
	Given start new match for home team "h1" and away team "a1"
	And wait for 1 second
	And start new match for home team "h2" and away team "a2"
	And wait for 1 second
	And start new match for home team "h3" and away team "a3"
	And update home team "h1" score to 3 and away team "a1" to 2
	And update home team "h2" score to 1 and away team "a2" to 0
	And update home team "h3" score to 2 and away team "a3" to 3
	When get match summery
	Then match beween home team "h1" and away team "a1" should be in index 1
	And match beween home team "h2" and away team "a2" should be in index 2
	And match beween home team "h3" and away team "a3" should be in index 0