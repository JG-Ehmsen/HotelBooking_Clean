Feature: CreateBooking
	In order to avoid silly mistakes
	As a general idiot
	I want to be able to create a booking

@mytag
Scenario Outline: Create booking
	Given I have entered a start date <startDate>
	And I have entered a end date <endDate>
	And I have entered a customer id <CustomerId>
	When I press button Create booking
	Then The result should be <expectedResult>

	Examples:
	| id  | CustomerId | startDate    | endDate      | expectedResult |
	| 47  | 39         | '2020-01-05' | '2020-01-05' | true           |
	| 79  | 102        | '2021-02-10' | '2021-03-11' | true           |
	| 112 | 1          | '2022-04-04' | '2022-05-05' | true           |
	| 113 | 2          | '1995-04-04' | '1995-05-05' | false          |