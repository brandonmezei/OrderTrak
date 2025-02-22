using Microsoft.AspNetCore.Authorization;

namespace OrderTrak.API.Providers.AuthRequirements
{
    public class FunctionAccessRequirement : IAuthorizationRequirement
    {
        public string FunctionName { get; }

        public FunctionAccessRequirement(string functionName)
        {
            FunctionName = functionName;
        }
    }

}
