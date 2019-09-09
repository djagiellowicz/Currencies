using Microsoft.AspNetCore.Identity;
using System;
using WalutyBusinessLogic.Models.Enums;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public static class DefaultRolesInitialization
    {
        public static async void Init(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(RolesEnum)))
            {
                bool doesRoleExist = await roleManager.RoleExistsAsync(roleName);

                if (!doesRoleExist)
                {
                   await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
