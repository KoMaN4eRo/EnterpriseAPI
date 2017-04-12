FORMAT: 1A

EnterpriseAPI application

# Main information
+ HOST: http://localhost:3128/
+ OPEN: "Visual studio 2015" to open this project
+ USE: This application give you ability to create and structured your Organizations. Each user has his own organizations and there is no way for him to embrace on another organizations. Each user have login via LinkedIn by means of using methods inside AccountController.


# Structure of Organization
## Entities Organization have  a hierarchical tree structure and it is give ypu abbility to organizeassets in your enterprise. So, You will have following entities inside the system:
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
So you want use our API. Offcourse you may make mistakes, and in this case you will get on of Error in Error list below
## Error list
+ Name error
 ++ NameErro1  {This name is already exist inside }
++ NameError2 {Name is expty}
++ NameError3 {There is no [concrete entity] with name [concrete name]}
