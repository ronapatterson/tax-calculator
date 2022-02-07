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

## How to Setup User Secrets
1. Open the Solution Explorer in Visual Studio 
2. Then right click on the 'Infrastructure' project, and select 'Manage User Secrets' as shown below

![ManageUserSecrets](https://user-images.githubusercontent.com/51467659/152818253-484b5ee6-fbd5-4d89-81f6-206a6ac525c8.PNG)

4. Your secrets file will now be created 

![ApiKey](https://user-images.githubusercontent.com/51467659/152818300-0b990597-30f6-473d-8538-6e05c74c4f82.PNG)

## How to Run Automated Tests
1. Open Test Explorer in Visual Studio 
2. Click the play all button as shown

![TestExplorer](https://user-images.githubusercontent.com/51467659/152818345-adad09cf-cd2f-4ecd-9ad6-fe1291a92608.PNG)
