# Sportradar Live Football World Cup Score Board
This is an onborading project for Sportradar company.

## Requirements
You are working in a sports data company, and we would like you to develop a new Live Football World Cup Scoreboard library that shows all the ongoing matches and their scores.

The scoreboard supports the following operations:

1. Start a new match, assuming initial score 0 – 0 and adding it the scoreboard.
This should capture following parameters:
a. Home team
b. Away team

2. Update score. This should receive a pair of absolute scores: home team score and away
team score.

3. Finish match currently in progress. This removes a match from the scoreboard.

4. Get a summary of matches in progress ordered by their total score. The matches with the
same total score will be returned ordered by the most recently started match in the
scoreboard.
