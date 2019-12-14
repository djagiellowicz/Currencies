using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace WalutyMVCWebApp.AuthorizeAttributes
{

        public class AuthorizeEnumRoles : AuthorizeAttribute
        {
        public AuthorizeEnumRoles(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("One of passed objects is not Enum type");

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }
    }
}
