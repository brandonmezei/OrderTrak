﻿namespace OrderTrak.API.Providers.AuthRequirements
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using OrderTrak.API.Models.OrderTrakDB;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class FunctionAccessHandler(OrderTrakContext dbContext) : AuthorizationHandler<FunctionAccessRequirement>
    {
        private readonly OrderTrakContext _dbContext = dbContext;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionAccessRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                context.Fail();
                return;
            }

            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
            {
                context.Fail();
                return;
            }

            var user = await _dbContext.SYS_Users
                .Include(x => x.SYS_Roles)
                    .ThenInclude(x => x.SYS_RolesToFunction)
                        .ThenInclude(x => x.SYS_Function)
                .FirstOrDefaultAsync(x => x.Email == email && x.Approved && x.RoleID.HasValue);

            if (user == null)
            {
                context.Fail();
                return;
            }

            var hasAccess = user.SYS_Roles.SYS_RolesToFunction
                .Any(x => x.CanAccess && requirement.AllowedFunctions.Contains(x.SYS_Function.FunctionName));

            if (hasAccess)
                context.Succeed(requirement);
            else
                context.Fail();
        }


    }

}
