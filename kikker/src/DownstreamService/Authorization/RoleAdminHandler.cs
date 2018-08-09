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
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }
            var role = context.User
            .FindFirst(claim => claim.Type == ClaimTypes.Role).Value.ToLower();
            //check admin role
            if (role == "administrator")
            {
                context.Succeed(requirement);
            }
            //check user role
            if (role == requirement.RoleName.ToLower())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
