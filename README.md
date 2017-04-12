FORMAT: 1A

EnterpriseAPI application

# Main information
+ HOST: http://localhost:3128/
+ OPEN: "Visual studio 2015" to open this project
+ USE: This application give you ability to create and structured your Organizations. Each user has his own organizations and there is no way for him to embrace on another organizations. Each user have login via LinkedIn by means of using methods inside AccountController.


# Structure of Organization
## Organization have  a hierarchical tree structure and it is give ypu abbility to organizeassets in your enterprise. So, You will have following entities inside the system:
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
1. + Organization (contain list of "Country")
2. + Country (contain list of "Business")
3. + Business (contain list of "Family")
4. + [Family] (contain list of "Offering")
5. + Offering (contain list of "Department")
6. + Department  
