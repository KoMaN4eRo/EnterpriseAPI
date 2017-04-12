using EnterpriseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Validation
{
    public class ModelValidation: IValidation
    {
        private ApplicationContext dbContext;

        public ModelValidation(ApplicationContext context)
        {
            dbContext = context;
        }

        public bool IsNull(object sender)
        {
            if (sender == null)
                return true;
            else
                return false;
        }

        public ModelStateHandler IsNullAll(ModelStateHandler handler, string name = null, string code = null, string type = null)
        {
            if (IsNull(name) && IsNull(code) && IsNull(type))
            {
                handler.modelValid = false;
                handler.modelState.Add("NullParam", "All entering parameters are empty");
                return handler;
            }

            else return handler;
        }

        public async Task<ModelStateHandler> CheckCode(string code, string method, string entity,
                                                       ModelStateHandler handler, string id = null)
        {
            if (method.Equals("Create"))
            {
                if (IsNull(code))
                {
                    handler.modelValid = false;
                    handler.modelState.Add("CodeError2", $"{entity}'s code is empty or 0");
                    return handler;
                }
            }


            if (!IsNull(code))
            {
                if (!(code.All(c => char.IsDigit(c))))
                {
                    handler.modelValid = false;
                    handler.modelState.Add("CodeError4", $"{entity}'s code contain non numerical sequence");
                    return handler;
                }

                if (int.Parse(code) == 0)
                {
                    handler.modelValid = false;
                    handler.modelState.Add("CodeError2", $"{entity}'s code is empty or 0");
                    return handler;
                }

                if (int.Parse(code) < 0)
                {
                    handler.modelValid = false;
                    handler.modelState.Add("CodeError3", $"{entity}'s code have to be positive");
                    return handler;
                }

                if (entity.Equals("Organization"))
                {
                    if (await dbContext.organization.AnyAsync(org => org.organizationCode == int.Parse(code)))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("CodeError1", $"{entity} with code:{code} already exist");
                        return handler;
                    }
                }

                //Check the same code
                if (entity.Equals("Country"))
                {
                    if (await dbContext.country.AnyAsync(c => c.countryCode == int.Parse(code) && c.organizationId == int.Parse(id)))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("CodeError1", $"{entity} with code:{code} already exist inside organization with id:{id}");
                        return handler;
                    }
                }
            }
            
            return handler;
        }

        public async Task<ModelStateHandler> CheckId(string id, string entity, string method, ModelStateHandler handler)
        {
            if (IsNull(id))
            {
                handler.modelValid = false;
                handler.modelState.Add("IdError1", $"{entity}'s id is empty");
                return handler;
            }

            if (!(id.All(c => char.IsDigit(c))))
            {
                handler.modelValid = false;
                handler.modelState.Add("IdError5", $"{entity}'s id contain non numerical sequence");
                return handler;
            }

            if (int.Parse(id) < 0)
            {
                handler.modelValid = false;
                handler.modelState.Add("IdError2", $"{entity}'s id have to be positive");
                return handler;
            }

            if (int.Parse(id) == 0)
            {
                handler.modelValid = false;
                handler.modelState.Add("IdError3", $"{entity}'s id is 0 ");
                return handler;
            }

            if (true)
            {
                if (entity.Equals("Organization"))
                {
                    if (!(await dbContext.organization.AnyAsync(o => o.organizationId == int.Parse(id))))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("IdError4", $"There is no {entity} with id {id}");
                        return handler;
                    }
                }

                if (entity.Equals("Country"))
                {
                    if (!(await dbContext.country.AnyAsync(c => c.countryId == int.Parse(id))))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("IdError4", $"There is no {entity} with id {id}");
                        return handler;
                    }
                }

                if (entity.Equals("Business"))
                {
                    if (!(await dbContext.business.AnyAsync(bus => bus.businessId == int.Parse(id))))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("IdError4", $"There is no {entity} with id {id}");
                        return handler;
                    }
                }

                if (entity.Equals("Family"))
                {
                    if (!(await dbContext.family.AnyAsync(fam => fam.familyId == int.Parse(id))))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("IdError4", $"There is no {entity} with id {id}");
                        return handler;
                    }
                }

                if (entity.Equals("Offering"))
                {
                    if (!(await dbContext.offering.AnyAsync(off => off.offeringId == int.Parse(id))))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("IdError4", $"There is no {entity} with id {id}");
                        return handler;
                    }
                }

                if (entity.Equals("Department"))
                {
                    if (!(await dbContext.department.AnyAsync(dep => dep.departmentId == int.Parse(id))))
                    {
                        handler.modelValid = false;
                        handler.modelState.Add("IdError4", $"There is no {entity} with id {id}");
                        return handler;
                    }
                }
            }

            return handler;
        }

        public async Task<ModelStateHandler> CheckName(string name, string entity, ModelStateHandler handler, 
                                                       string method = null, string id = null)
        {
            if (!method.Equals("Update"))
            {
                if (IsNull(name))
                {
                    handler.modelValid = false;
                    handler.modelState.Add("NameError2", $"{entity}'s name is empty");
                    return handler;
                }
            }

            if (method.Equals("Update") && !(entity.Equals("Country") || entity.Equals("Organization")))
            {
                if (IsNull(name))
                {
                    handler.modelValid = false;
                    handler.modelState.Add("NameError2", $"{entity}'s name is empty");
                    return handler;
                }
            }

            if (!IsNull(name))
            {
                if (method.Equals("Delete"))
                {
                    if (entity.Equals("Organization"))
                    {
                        if (!(await dbContext.organization.AnyAsync(o => o.organizationName.Equals(name))))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError3", $"There is no {entity.ToLower()} with name {name}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Country"))
                    {
                        if (!(await dbContext.country.AnyAsync(c => c.countryName == name)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError3", $"There is no {entity.ToLower()} with name {name}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Business"))
                    {
                        if (!(await dbContext.business.AnyAsync(bus => bus.businessName == name)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError3", $"There is no {entity.ToLower()} with name {name}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Family"))
                    {
                        if (!(await dbContext.family.AnyAsync(fam => fam.familyName == name)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError3", $"There is no {entity.ToLower()} with name {name}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Offering"))
                    {
                        if (!(await dbContext.offering.AnyAsync(off => off.offeringName == name)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError3", $"There is no {entity.ToLower()} with name {name}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Department"))
                    {
                        if (!(await dbContext.department.AnyAsync(dep => dep.departmentName == name)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError3", $"There is no {entity.ToLower()} with name {name}");
                            return handler;
                        }
                    }
                }

                if (!method.Equals("Delete"))
                {
                    if (entity.Equals("Organization"))
                    {
                        if (await dbContext.organization.AnyAsync(org => org.organizationName == name))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError1", $"{entity} with name:{name} already exist");
                            return handler;
                        }
                    }

                    if (entity.Equals("Country"))
                    {
                        if (await dbContext.country.AnyAsync(cont => cont.countryName == name && cont.organizationId == int.Parse(id)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError1", $"{entity} with name:{name} already exist inside organization with id:{id}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Business"))
                    {
                        if (await dbContext.business.AnyAsync(buss => buss.businessName == name && buss.countryId == int.Parse(id)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError1", $"{entity} with name:{name} already exist inside country with id:{id}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Family"))
                    {
                        if (await dbContext.family.AnyAsync(fam => fam.familyName == name && fam.businessId == int.Parse(id)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError1", $"{entity} with name:{name} already exist inside business with id:{id}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Offering"))
                    {
                        if (await dbContext.offering.AnyAsync(off => off.offeringName == name && off.familyId == int.Parse(id)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError1", $"{entity} with name:{name} already exist inside family with id:{id}");
                            return handler;
                        }
                    }

                    if (entity.Equals("Department"))
                    {
                        if (await dbContext.department.AnyAsync(dep => dep.departmentName == name && dep.offeringId == int.Parse(id)))
                        {
                            handler.modelValid = false;
                            handler.modelState.Add("NameError1", $"{entity} with name:{name} already exist inside family with id:{id}");
                            return handler;
                        }
                    }
                }                
            }
                   
           return handler;
        }

        public async Task<ModelStateHandler> CheckType(string type, ModelStateHandler handler, string method = "Get")
        {
            if (!method.Equals("Update"))
            {
                if (IsNull(type))
                {
                    handler.modelValid = false;
                    handler.modelState.Add("TypeError1", $"Organization's type is empty");
                    return handler;
                }
            }

            if (method.Equals("Get"))
            {
                if (!(await dbContext.organization.AnyAsync(o => o.organizationType.Equals(type))))
                {
                    handler.modelValid = false;
                    handler.modelState.Add("TypeError2", $"There is no organization with type {type}");
                    return handler;
                }
            }                       

            return handler;
        }
    }
}
