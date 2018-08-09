using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string roleName)
        {
            RoleName = roleName;
        }

        public string RoleName { get; set; }
    }
}
