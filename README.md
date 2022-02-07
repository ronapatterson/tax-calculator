# Tax-Calculator

This program was built as a requirement of IMC Digital Innovstions, to calculate taxes via api calls to tax calculators. It was written using the clean architecture principles, and implements loose coupling (via abstractions e.g. interfaces) from implementations. As such the classes that are implementations are set to an 'internal' scope for accessability, to assist in preventing developers from doing the wrong thing such as calling implementation methods directly from other layers. 

This project comprises of the following:
- Infrastructure
- Application
- Domain
- Integration Tests
- Unit Tests
- WebApi (For End to End testing manually)

## Steps to Run Project
1. Setup user secrets in the below format:
`
"TaxCalculatorApis":{
      "TaxJar": "api url"
}
"ApiKeys":{
    "TaxJar": "api key"
}

2. Run WebApi projecct to test manually
*Automated tests are also included for integration, unit testing
`
