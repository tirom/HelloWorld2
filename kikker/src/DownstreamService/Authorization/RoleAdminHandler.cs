using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Authorization
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            //if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            if (!context.User.HasClaim(c => c.Type == "auth"))
            {
                return Task.CompletedTask;
            }
            var role = context.User
            .FindFirst(claim => claim.Type == "auth").Value;
            //check admin role
            if (role.Contains("ROLE_ADMIN"))
            {
                context.Succeed(requirement);
            }
            //check user role
            if (role.Contains(requirement.RoleName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
