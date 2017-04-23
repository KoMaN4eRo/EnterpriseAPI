using System.Threading.Tasks;

namespace EnterpriseAPI.Validation.ValidateOrganization.Code
{
    public interface IValidateCode
    {
        Task<bool> IsValidCode(int code);
    }
}