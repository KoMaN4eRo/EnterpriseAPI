using System.Threading.Tasks;

namespace EnterpriseAPI.Validation
{
    public interface IValidation
    {
        bool IsNull(object sender);
        ModelStateHandler IsNullAll(ModelStateHandler handler, string name = null, string code = null, string type = null);
        Task<ModelStateHandler> CheckCode(string code, string method, string entity, ModelStateHandler handler, string id = null);
        Task<ModelStateHandler> CheckId(string id, string entity, string method, ModelStateHandler handler);
        Task<ModelStateHandler> CheckName(string name, string entity, ModelStateHandler handler,
                                                       string method = "Default", string id = null);
        Task<ModelStateHandler> CheckType(string type, ModelStateHandler handler, string method = "Default");
    }
}
