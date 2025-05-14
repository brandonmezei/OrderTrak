using Microsoft.AspNetCore.Authorization;

namespace OrderTrak.API.Providers.AuthRequirements
{
    public class FunctionAccessRequirement : IAuthorizationRequirement
    {
        public HashSet<string> AllowedFunctions  { get; }

        public FunctionAccessRequirement(params string[] functionNames)
        {
            AllowedFunctions  = new HashSet<string>(functionNames);
        }
    }

}
