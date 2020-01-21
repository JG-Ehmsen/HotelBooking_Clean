Feature: GetFullyOccupiedDates
	In order to avoid silly mistakes
	As a math idiot
	I want to get the amount of fully occupied dates

@mytag
Scenario Outline: Get fully occupied dates
	Given I have entered a date range start date <startDate>
	And I have entered a date range end date <endDate>
	When I press find fully occupied dates
	Then the result should be <expectedFullyOccupiedDates>

	Examples:
	| id | startDate    | endDate      | expectedFullyOccupiedDates |
	| 1  | '2020-01-01' | '2020-01-01' | 1                          |
	| 2  | '2020-01-01' | '2020-01-05' | 5                          |
	| 3  | '2020-01-01' | '2021-01-01' | 367                        |
	| 4  | '2040-01-01' | '2041-01-01' | 0                          |