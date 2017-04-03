FORMAT: 1A
HOST: http://localhost:3128/

# EnterpriseAPI description


## Sign in via LinkedIn [/login]
### Login [GET]
You will get this answer if you first time sign in, or if you delete yourself from 
database
+ Response 200 (application/json; charset=utf-8)

        [
            "200"
        ]
        

+ Response 200 (application/json; charset=utf-8)

        [
            "User -name- -surname- already exist"
        ]        
        
## Logout [/logout]
### Login [GET]
After this action you will be redirect to LinkedIn official web site
+ Response 200 (application/json; charset=utf-8)

        [
            "You has been logout successfuly"
        ]        
        
## Put data to user profile [/api/Account/Put/?{address}]
This method available only after sign in
+ Parameters
    + address (string) - Real user address

### Put user real address [PUT]

+ Response 200 (application/json)

        {
            "User profile has been updated successfully"
        }

## Get user [/api/Account/Get/]

### Get user profile [GET]

+ Response 200 (application/json)

        {
            "userId": 3,
             "name": "Pavel",
             "lastName": "Levchenko",
             "email": "dromedar@ukr.net",
             "address": "Street"
        }
        
## Delete user [/api/Account/Delete/]


### Delete current user profile [Delete]

+ Response 200 (application/json)

        {
            "User with id:3 has been successfully deleted"
        }        
        
Organization Managing

## Create organization [/api/Organization/Create/?{name}&{orgCode}&{type}]
This method available only after sign in. 
+ Parameters
    + name (string) - organization name, have to be unique
    +orgCode (int) - organization Code, have to be unique
    +type (string) - type of organizaion

### Create organization [POST]

+ Response 200 (application/json)

        {
            "200"
        }
        
+ Response 200 (application/json)

        {
            "Organization with name:odin already exist"
        }

+ Response 200 (application/json)

        {
            "Organization with code:193 already exist"
        }

+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

## Get all organizations and bottom levels [/api/Organization/ExpandAll]

### ExpandAll [GET]

+ Response 200 (application/json)

        
            [
              {
                "organizationId": 1,
                "organizationName": "Stupid",
                "organizationCode": 6971,
                "organizationType": "Limited partnership",
                "owner": "Pasha",
                "country": [
                  {
                    "countryId": 3,
                    "countryName": "Ukraine",
                    "countryCode": 38,
                    "organizationId": 1,
                    "business": [
                      {
                        "businessId": 1,
                        "businessName": "Small",
                        "countryId": 3,
                        "family": [
                          {
                            "familyId": 1,
                            "familyName": "Volk",
                            "businessId": 1,
                            "offering": [
                              {
                                "offeringId": 1,
                                "offeringName": "DICH",
                                "familyId": 1,
                                "department": [
                                  {
                                    "departmentId": 1,
                                    "departmentName": "Dep1",
                                    "offeringId": 1
                                  }
                                ]
                              }
                            ]
                          }
                        ]
                      }
                    ]
                  },
                  {
                    "countryId": 1003,
                    "countryName": "France",
                    "countryCode": 47,
                    "organizationId": 1,
                    "business": []
                  }
                ]
              },
              {
                "organizationId": 3,
                "organizationName": "Sasha",
                "organizationCode": 12345678,
                "organizationType": "General PartnerShip",
                "owner": "Bill",
                "country": []
              },
              {
                "organizationId": 4,
                "organizationName": "Sashok",
                "organizationCode": 10234,
                "organizationType": "Limited Liability",
                "owner": "SATAN",
                "country": []
              },
              {
                "organizationId": 6,
                "organizationName": "Microsoft",
                "organizationCode": 9965,
                "organizationType": "IncorIncorporated company",
                "owner": null,
                "country": []
              },
              {
                "organizationId": 7,
                "organizationName": "Max",
                "organizationCode": 97,
                "organizationType": "General",
                "owner": "Coleno",
                "country": []
              },
              {
                "organizationId": 1007,
                "organizationName": "Special",
                "organizationCode": 321,
                "organizationType": "Hosted",
                "owner": "PavelLevchenko",
                "country": []
              },
              {
                "organizationId": 1008,
                "organizationName": "odin",
                "organizationCode": 193,
                "organizationType": "Corporation",
                "owner": "PavelLevchenko",
                "country": []
              }
            ]
        
## Get all organizations [/api/Organization/Get]

### ExpandAll [GET]
+ Response 200 (application/json)

        [
          {
            "organizationId": 1,
            "organizationName": "Stupid",
            "organizationCode": 6971,
            "organizationType": "Limited partnership",
            "owner": "Pasha",
            "country": null
          },
          {
            "organizationId": 3,
            "organizationName": "Sasha",
            "organizationCode": 12345678,
            "organizationType": "General PartnerShip",
            "owner": "Bill",
            "country": null
          },
          {
            "organizationId": 4,
            "organizationName": "Sashok",
            "organizationCode": 10234,
            "organizationType": "Limited Liability",
            "owner": "SATAN",
            "country": null
          },
          {
            "organizationId": 6,
            "organizationName": "Microsoft",
            "organizationCode": 9965,
            "organizationType": "IncorIncorporated company",
            "owner": null,
            "country": null
          },
          {
            "organizationId": 7,
            "organizationName": "Max",
            "organizationCode": 97,
            "organizationType": "General",
            "owner": "Coleno",
            "country": null
          },
          {
            "organizationId": 1007,
            "organizationName": "Special",
            "organizationCode": 321,
            "organizationType": "Hosted",
            "owner": "PavelLevchenko",
            "country": null
          },
          {
            "organizationId": 1008,
            "organizationName": "odin",
            "organizationCode": 193,
            "organizationType": "Corporation",
            "owner": "PavelLevchenko",
            "country": null
          }
        ]
        
## Get by type [/api/Organization/GetByType/?{organizationType}]
    + Parameters
        + organizationType (string) - organization type, 
Show organizations with specific  type
### ExpandAll [GET]
+ Response 200 (application/json)

        [
          {
            "organizationId": 4,
            "organizationName": "Sashok",
            "organizationCode": 10234,
            "organizationType": "Limited Liability",
            "owner": "SATAN",
            "country": null
          }
        ]

## Update organization [/api/Organization/Put/?{id}&{name}&{orgCode}&{type}]
    + Parameters
        + id (int) - organization id,
        + name (int) - new organization name, (have to be unique)
        + orgCode (string) - new organization type, (have to be unique)
        + type (string) - organization type, any type you want
Show organizations with specific  type
### Update organization INFO [PUT]
+ Response 200 (application/json)

        [
          "200"
        ]

+ Response 200 (application/json)

        [
          "Organization with name:newname already exist"
        ]
        
+ Response 200 (application/json)

        [
          "Organization with code:986 already exist"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]        
## Delete organization [/api/Organization/Delete/?{name}]
    + Parameters
        + name (string)- the name of organization? you want to delete

### Delete organization INFO [Delete]
+ Response 200 (application/json)

        [
          "200"
        ]
        
+ Response 200 (application/json)

        [
          "There is no organization with name odin"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

Country Managing

## Create Country [/api/Country/Create/?{name}&{countryCode}&{orgId}]
    + Parameters
        +name (string) - country name, have to be unique
        +countryCode (int) - country Code, have to be unique
        +orgId (int) - organization id that wil containt country you want to add

### Create country [POST]

+ Response 200 (application/json)

        {
            "200"
        }
        
+ Response 200 (application/json)

        {
            "Country with name:idf already exist inside organization with id:1"
        }

+ Response 200 (application/json)

        {
            "Country with code:78 already exist inside organization with id:1"
        }

+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

## Get all organizations and bottom levels [/api/Country/ExpandAll/?{orgId}]
    + Parameters
        +orgId (int) - expand all bottom levels containt in current organization
### ExpandAll [GET]

+ Response 200 (application/json)

        
            [
              {
                "countryId": 3,
                "countryName": "Ukraine",
                "countryCode": 38,
                "organizationId": 1,
                "business": [
                  {
                    "businessId": 1,
                    "businessName": "Small",
                    "countryId": 3,
                    "family": [
                      {
                        "familyId": 1,
                        "familyName": "Volk",
                        "businessId": 1,
                        "offering": [
                          {
                            "offeringId": 1,
                            "offeringName": "DICH",
                            "familyId": 1,
                            "department": [
                              {
                                "departmentId": 1,
                                "departmentName": "Dep1",
                                "offeringId": 1
                              }
                            ]
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                "countryId": 1003,
                "countryName": "France",
                "countryCode": 47,
                "organizationId": 1,
                "business": []
              },
              {
                "countryId": 2003,
                "countryName": "idf",
                "countryCode": 78,
                "organizationId": 1,
                "business": []
              }
            ]

## Get all Country without bottom levels [/api/Country/Get/?{orgId}]
    + Parameters
        +orgId (int) -  target organization Id
### Get country [GET]
+ Response 200 (application/json)

        [
          {
            "countryId": 3,
            "countryName": "Ukraine",
            "countryCode": 38,
            "organizationId": 1,
            "business": null
          },
          {
            "countryId": 1003,
            "countryName": "France",
            "countryCode": 47,
            "organizationId": 1,
            "business": null
          },
          {
            "countryId": 2003,
            "countryName": "idf",
            "countryCode": 78,
            "organizationId": 1,
            "business": null
          }
        ]
        
## Update country [/api/Country/Put/?{id}&{orgId}&{id}&{name}&{code}]
    + Parameters
        + id (int) - country id,
        + orgId (int) -  organization id
        + name (string) - new country name, (have to be unique)
        + code (int) - country code
### Update country INFO [PUT]
+ Response 200 (application/json)

        [
          "200"
        ]

+ Response 200 (application/json)

        [
          "Country with name:blalba already exist inside organization with id:1"
        ]
        
+ Response 200 (application/json)

        [
          "Country with code:96 already exist inside organization with id:1"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]        
## Delete country [/api/country/Delete/?{name}&{orgId}]
    + Parameters
        + name (string)- the name of organization? you want to delete
        + orgId (string) - organization id, that contain this country
### Delete country INFO [Delete]
+ Response 200 (application/json)

        [
          "200"
        ]
        
+ Response 200 (application/json)

        [
          "There is no country with name France and organizationId 1"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]
        
BUsiness Managing

## Create Business [/api/Business/Create/?{name}&{countryId}]
    + Parameters
        +name (string) - country name, have to be unique
        +countryId (int) - place here country Id inside which you want to add business 

### Create business [POST]

+ Response 200 (application/json)

        {
            "200"
        }
        
+ Response 200 (application/json)

        {
            "Business with name:Franceksdjbfs already exist inside country with id:3"
        }

+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

## Get all business and bottom levels [/api/Business/ExpandAll/?{businessd}]
    + Parameters
        +businessId (int) - expand all bottom levels containt in current organization
### ExpandAll [GET]

+ Response 200 (application/json)

        
           [
              {
                "businessId": 1,
                "businessName": "Small",
                "countryId": 3,
                "family": [
                  {
                    "familyId": 1,
                    "familyName": "Volk",
                    "businessId": 1,
                    "offering": [
                      {
                        "offeringId": 1,
                        "offeringName": "DICH",
                        "familyId": 1,
                        "department": [
                          {
                            "departmentId": 1,
                            "departmentName": "Dep1",
                            "offeringId": 1
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                "businessId": 2,
                "businessName": "Franceksdjbfs",
                "countryId": 3,
                "family": []
              }
            ]
        
## Get all Business without bottom levels [/api/Business/Get/?{businessId}]
    + Parameters
        +businessId (int) -  target organization Id
### Get country [GET]
+ Response 200 (application/json)

        [
          {
            "businessId": 1,
            "businessName": "Small",
            "countryId": 3,
            "family": null
          },
          {
            "businessId": 2,
            "businessName": "Franceksdjbfs",
            "countryId": 3,
            "family": null
          }
        ]

## Update Business [/api/Business/Put/?{businessId}&{id}&{name}]
    + Parameters
        + countryId  - target country Id
        + id  - target business
        + name - new name
### Update country INFO [PUT]
+ Response 200 (application/json)

        [
          "200"
        ]

+ Response 200 (application/json)

        [
          "Business with name:noss already exist inside country with id:3"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]        
## Delete country [/api/Business/Delete/?{name}&{countryId}]
    + Parameters
        + name (string)- the name of business
        + countryId (string) - countryId id
### Delete country INFO [Delete]
+ Response 200 (application/json)

        [
          "200"
        ]
        
+ Response 200 (application/json)

        [
          -"There is no business with name {name} and countryId {countryId}"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]
        
Family Managing

## Create family [/api/Business/Create/?{name}&{businessId}]
This method available only after sign in. 
    + Parameters
        +name (string) - country name, have to be unique
        +businessId (int) - expand all bottom levels containt in current organization

### Create business [POST]

+ Response 200 (application/json)

        {
            "200"
        }
        
+ Response 200 (application/json)

        {
            "Family with name:kjahdfb already exist inside business with id:1"
        }

+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

## Get all business and bottom levels [/api/Family/ExpandAll/?{businessd}]
    + Parameters
        +businessId (int) - expand all bottom levels containt in current organization
### ExpandAll [GET]

+ Response 200 (application/json)

        
           [
              {
                "familyId": 1,
                "familyName": "Volk",
                "businessId": 1,
                "offering": [
                  {
                    "offeringId": 1,
                    "offeringName": "DICH",
                    "familyId": 1,
                    "department": [
                      {
                        "departmentId": 1,
                        "departmentName": "Dep1",
                        "offeringId": 1
                      }
                    ]
                  }
                ]
              },
              {
                "familyId": 2,
                "familyName": "kjahdfb",
                "businessId": 1,
                "offering": []
              }
            ]
        
## Get all Family without bottom levels [/api/Family/Get/?{businessId}]
    + Parameters
        +businessId (int) -  target business Id
### Get country [GET]
+ Response 200 (application/json)

        [
          {
            "familyId": 1,
            "familyName": "Volk",
            "businessId": 1,
            "offering": null
          },
          {
            "familyId": 2,
            "familyName": "kjahdfb",
            "businessId": 1,
            "offering": null
          }
        ]

## Update Business [/api/Family/Put/?{businessId}&{id}&{name}]
    You can paste here name or code or both of them
    + Parameters
        + businessId  - target family Id
        + id  - target business
        + name - new name
### Update country INFO [PUT]
+ Response 200 (application/json)

        [
          "200"
        ]

+ Response 200 (application/json)

        [
          "Family with name:{name} already exist inside business with id:{id}"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]        
## Delete country [/api/Family/Delete/?{name}&{businessId}]
    + Parameters
        + name (string)- the name of business
        + businessId (string) - countryId id
### Delete country INFO [Delete]
+ Response 200 (application/json)

        [
          "200"
        ]
        
+ Response 200 (application/json)

        [
          -"There is no family with name {name} and businessId {id}"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

Offering Managing

## Create Offering [/api/Offering/Create/?{name}&{familyId}]
This method available only after sign in. 
    + Parameters
        +name (string) - Offering name, have to be unique
        +familyId (int) - expand all bottom levels containt in current family

### Create business [POST]

+ Response 200 (application/json)

        {
            "200"
        }
        
+ Response 200 (application/json)

        {
            "Offering with name:{name} already exist inside business with id:{id}"
        }

+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

## Get all business and bottom levels [/api/Offering/ExpandAll/?{familyd}]
    + Parameters
        +familyId (int) - expand all bottom levels containt in current family
### ExpandAll [GET]

+ Response 200 (application/json)

        
           [
              {
                "offeringId": 1,
                "offeringName": "DICH",
                "familyId": 1,
                "department": [
                  {
                    "departmentId": 1,
                    "departmentName": "Dep1",
                    "offeringId": 1
                  }
                ]
              }
            ]
        
## Get all Offering without bottom levels [/api/Offering/Get/?{familyId}]
    Show all offering iside famnily 
    + Parameters
        +familyId (int) -  target family Id
### Get country [GET]
+ Response 200 (application/json)

        [
          {
            "offeringId": 1,
            "offeringName": "DICH",
            "familyId": 1,
            "department": null
          }
        ]

## Update Business [/api/Offering/Put/?{family}&{id}&{name}]
This method available only after sign in
    + Parameters
        + familyID  - target family Id
        + id  - target offering
        + name - new name
### Update country INFO [PUT]
+ Response 200 (application/json)

        [
          "200"
        ]

+ Response 200 (application/json)

        [
          "Offering with name:{name} already exist inside business with id:{id}"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]        
## Delete country [/api/Offering/Delete/?{name}&{offeringId}]
    + Parameters
        + name (string)- the name of business
        + offeringId (string) - offering id
### Delete country INFO [Delete]
+ Response 200 (application/json)

        [
          "200"
        ]
        
+ Response 200 (application/json)

        [
          -"There is no offering with name {name} and businessId {id}"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

Department Managing

## Create Department [/api/Department/Create/?{name}&{offeringId}]
This method available only after sign in. 
    + Parameters
        +name (string) - depart. name
        +offering (int) - 

### Create business [POST]

+ Response 200 (application/json)

        {
            "200"
        }
        
+ Response 200 (application/json)

        {
            "Department with name:{name} already exist inside offering with id:{id}"
        }

+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]

## Get Department [/api/Department/Get/?{OfferingId}]
    Show all offering iside famnily 
    + Parameters
        +Offering (int) -  target family Id
### Get country [GET]
+ Response 200 (application/json)

        [
          {
            "departmentId": 1,
            "departmentName": "Dep1",
            "offeringId": 1
          }
        ]

## Update Business [/api/Department/Put/?{familyId}&{id}&{name}]
This method available only after sign in
    + Parameters
        + familyID  - target offering Id
        + id  - target department
        + name - new name
### Update Department INFO [PUT]
+ Response 200 (application/json)

        [
          "200"
        ]

+ Response 200 (application/json)

        [
          "Department with name:{name} already exist inside offering with id:{id}"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]        
## Delete Department [/api/Department/Delete/?{name}&{familyId}]
    + Parameters
        + name (string)- the name of business
        + familyId (string) - countryId id
### Delete Department INFO [Delete]
+ Response 200 (application/json)

        [
          "200"
        ]
        
+ Response 200 (application/json)

        [
          -"There is no department with name {name} and offering {id}"
        ]
        
+ Response 200 (application/json)

        [
          "Error. Please Authenticate via social network"
        ]
        
