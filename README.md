# Price Calculator

## Description
A .net core 3 console application to calculate the price of a basket of shopping

### Supported products
* Apples
* Beans
* Bread
* Milk

### Usage 
`PriceCalculator item1 item2 item3`

e.g.

`PriceCalculator Apples Milk Bread`

## Implementation 

### Assumptions
* No overlapping offers - e.g. an item will not appear in two offers at the same time
* Item names are case insensitive

### Notes
* Not all classes are tested for the sake of brevity (the repos are not particulartly interesting)

### Possible extensions
* Change logging provider to log to file
* Replace repos with database repos - e.g. EF Core
* Implement proper Date Provider functionality to allow mocking of datetimes for the rule expiry (at present the rule will never expire)
* Using a dictionary to hold the basket made calculating offers more complicated than using another data structure
    * would probable refactor to use a new BasketItem type
* Extract the offer calculations to their own classes and inject
    * Calculation of offers is really a rules engine and gets complicated quickly especially if rules can overlap
