using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.Enums;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public static class DefaultAdminCreator
    {
        private static readonly string defaultAdminName = "administrator@adm.com";
        private static readonly string defaultAdminPassword = "!1234Qwertyuiop";

        public async static void CreateAdmin(UserManager<User> userManager)
        {
            if (await userManager.FindByNameAsync(defaultAdminName) == null)
            {
                var user = new User { UserName = defaultAdminName, Email = defaultAdminName, SecurityStamp = Guid.NewGuid().ToString(), UserFavoriteCurrencies = new List<UserCurrency>() };

                var result = await userManager.CreateAsync(user, defaultAdminPassword);           

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RolesEnum.Administrator.ToString());
                    await userManager.AddToRoleAsync(user, RolesEnum.User.ToString());

                    Log.Logger.Information($"User {defaultAdminName} has been created.");
                }
            }
        }
    }
}
