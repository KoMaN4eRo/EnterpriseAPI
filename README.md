FORMAT: 1A

EnterpriseAPI application

# MAIN INFORMATION
+ HOST: http://localhost:3128/
+ OPEN: "Visual studio 2015" to open this project
+ USE: This application give you ability to create and structured your Organizations. Each user has his own organizations and there is no way for them to embrace on another organizations. Each user have to login via LinkedIn by means of using methods inside AccountController.
+ TEST: For testing API you can use Postman Interceptor. For testing API turn on interceptor in browser. https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop?hl=en


# STRUCTURE OF PROJECT
## Entities Organization have  a hierarchical tree structure and it is give you abbility to organize assets in your enterprise. So, You will have following entities inside the system:
    1. User (Name, Surname, Email, Address)
    2. Country (Name, Code)
    3. Organization (Name, Code, Organization Type (General Partnership,
    Limited partnerships, Limited Liability Company (Co. Ltd.),
    Incorporated company, Social enterprise, Other), Owner).
    4. Business (Name)
    5. Family (depends on business)
    6. Offering (depends on family)
    7. Department (name)
    
## This is structure of classes
 + Organization (contain list of "Country")
 + Country (contain list of "Business")
 + Business (contain list of "Family")
 + Family (contain list of "Offering")
 + Offering (contain list of "Department")
 + Departme

## Validation
According to Entities and Structure of classes we can formed validation rules
+ Entity organization: [Name and Code have to be unique inisde Organization table in other way you will get Exception]
+ Entity country: [Name and Code have to be unique inisde concrete organization in other way you will get Exception]
+ Entity business: [Name have to be unique inisde concrete country in other way you will get Exception]
+ Entity family: [Name have to be unique inisde concrete business in other way you will get Exception]
+ Entity offering: [Name have to be unique inisde concrete family in other way you will get Exception]
+ Entity department: [Name have to be unique inisde concrete department in other way you will get Exception]


# USING OF API
So you want use our API. Certainly, you may make mistakes, and in this case you will get one of Error in list below
## Error list
+ NAME ERROR
	+ NameErro1  {This name is already exist}
	+ NameError2 {Name is expty}
	+ NameError3 {There is no [concrete entity] with name [concrete name]}
+ CODE ERROR (only for Organization and country)
	+ CodeError1 {This code is already exist}
	+ CodeError2 {Code is null or empty}
	+ CodeError4 {Code contain non numerical sequence}
+ TYPE ERROR (only for Organization)
	+ TypeError1 {Type is empty}
	+ TypeError2 {There is no organization with type [concrete type]}
+ ID ERROR
	+ IdError1 {Id is empty}
	+ IdError2 {Id have to be possitive}
	+ IdError3 {Id is 0}
	+ IdError4 {There is no [concrete entity] with id [conctete id]}
	+ IdError5 {Id contain non numerical sequence}
+ NULL ERROR
	+ NullParam {All entering parameters are empty}
+ AUTHENTICATION (This error appear when you try to use POST, DELETE or PUT method before you authenticate via LinkedIn. All get methods are available without authentication.)
	+ Please authorize via LinkedIn
## API
### ACCOUNT (application/json)
Methods available:
+ LOGIN 
	+ Description: Use this method, if you want to authenticate via LinkedIn. WARNING: Only after authentication you can use POST, PUT and DELETE methods
	+ Type: [GET]
	+ Address: [~/api/Account/Login] 
	+ Response:  
		+ Status code: 200
		+ Message: "Login complete"
	+ Error: If you already login and you try to use this method again, you will get 
		+ Status code: 404
+ LOGOUT
	+ Description: Use this method, if you want to logout
	+ Type: [GET]
	+ Address: [~/api/Account/Logout] 
	+ Response:  
		+ Status code: 200
		+ Message: "Logout complete"
+ PUT 	
	+ Description: Use this method, if you want to set you real address
	+ Type: [PUT]
	+ Address: [~/api/Account/Put/?{address}] 
	+ Parameters
		+ address (string) - your real address
	+ Response:  
		+ Status code: 200
		+ Message: "Logout complete"
	+ Error: 
		+ Status code: 200
		+ Message: "There is no value"
+ Get 
	+ Description: Use this method, if you want get iformation about your account
	+ Type: [GET]
	+ Address: [~/api/Account/Get] 
	+ Response:  
		+ Status code: 200
		+ Message: 
		
			{
			  "userId": 1,
			  "name": "Pavel",
			  "lastName": "Levchenko",
			  "email": "dromedar@ukr.net",
			  "address": null
			}
+ Delete 	
	+ Description: Use this method, if you want to delete your account
	+ Type: [Delete]
	+ Address: [~/api/Account/Delete] 
	+ Response:  
		+ Status code: 200
		+ Message: "Logout complete"

### ORGANIZATION (application/json)
Methods available:
+ CREATE 
	+ Description: Create organization
	+ Type: [POST]
	+ Address: [~/api/Organization/Create/?{name}&{code{&{type}] 
	+ Parameters
		+ name - Organization's name
		+ code - Organization's code
		+ type - Organization's type
	+ Response:  
		+ Status code: 200
		+ Message: {}
	+ Error: Description of all error, that you can receive in response containt in Error list above
+ EXPANDALL 
	+ Description: Give you all bottom levels that containt target Organization
	+ Type: [GET]
	+ Address: [~/api/Organization/ExpandAll/?{id}] 
	+ Parameters
		+ id - Organization's id
	+ Response:  
		+ Status code: 200
		+ Message: 
		{
  "organizationId": 2,
  "organizationName": "OON",
  "organizationCode": 266,
  "organizationType": "International",
  "owner": "Pavel Levchenko",
  "country": [
    {
      "countryId": 1,
      "countryName": "USA",
      "countryCode": 38,
      "organizationId": 2,
      "business": [
        {
          "businessId": 1,
          "businessName": "Cop",
          "countryId": 1,
          "family": [
            {
              "familyId": 1,
              "familyName": "My",
              "businessId": 1,
              "offering": [
                {
                  "offeringId": 1,
                  "offeringName": "New",
                  "familyId": 1,
                  "department": [
                    {
                      "departmentId": 1,
                      "departmentName": "Ider",
                      "offeringId": 1
                    },
                    {
                      "departmentId": 2,
                      "departmentName": "Dep2",
                      "offeringId": 1
                    },
                    {
                      "departmentId": 5,
                      "departmentName": "Dep3",
                      "offeringId": 1
                    },
                    {
                      "departmentId": 6,
                      "departmentName": "Dep4",
                      "offeringId": 1
                    },
                    {
                      "departmentId": 7,
                      "departmentName": "Dep5",
                      "offeringId": 1
                    },
                    {
                      "departmentId": 8,
                      "departmentName": "Dep6",
                      "offeringId": 1
                    }
                  ]
                },
                {
                  "offeringId": 2,
                  "offeringName": "New1",
                  "familyId": 1,
                  "department": []
                },
                {
                  "offeringId": 3,
                  "offeringName": "New2",
                  "familyId": 1,
                  "department": []
                },
                {
                  "offeringId": 4,
                  "offeringName": "New3",
                  "familyId": 1,
                  "department": []
                },
                {
                  "offeringId": 5,
                  "offeringName": "New4",
                  "familyId": 1,
                  "department": []
                },
                {
                  "offeringId": 6,
                  "offeringName": "New5",
                  "familyId": 1,
                  "department": []
                },
                {
                  "offeringId": 7,
                  "offeringName": "Data",
                  "familyId": 1,
                  "department": []
                }
              ]
            },
            {
              "familyId": 2,
              "familyName": "Name2",
              "businessId": 1,
              "offering": []
            },
            {
              "familyId": 3,
              "familyName": "Name3",
              "businessId": 1,
              "offering": []
            },
            {
              "familyId": 4,
              "familyName": "Name4",
              "businessId": 1,
              "offering": []
            },
            {
              "familyId": 5,
              "familyName": "Name5",
              "businessId": 1,
              "offering": []
            }
          ]
        },
        {
          "businessId": 2,
          "businessName": "Antoher1",
          "countryId": 1,
          "family": []
        },
        {
          "businessId": 3,
          "businessName": "Antoher2",
          "countryId": 1,
          "family": []
        },
        {
          "businessId": 4,
          "businessName": "Antoher3",
          "countryId": 1,
          "family": []
        },
        {
          "businessId": 5,
          "businessName": "Antoher4",
          "countryId": 1,
          "family": []
        },
        {
          "businessId": 6,
          "businessName": "Antoher5",
          "countryId": 1,
          "family": []
        }
      ]
    }
  ]
}
	+ Error: Description of all error, that you can receive in response containt in Error list above	
	
