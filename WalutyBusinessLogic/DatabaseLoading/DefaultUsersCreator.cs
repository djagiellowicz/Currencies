using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.Enums;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public class DefaultUsersCreator
    {
        private static readonly string[] defaultUserNames = new string[] { "kamr@example.com"
            , "zers@example.com", "kali@example.com"
            , "dromero@example.com", "karo@example.com"
            , "zajnb@example.com", "frank@example.com"
            , "stephen@example.com", "harry@example.com"};
        private static readonly string defaultUserPassword = "!1234Qwertyuiop";

        public async static void CreateUsers(UserManager<User> userManager)
        {
            foreach (var defaultUser in defaultUserNames)
            {
                if (await userManager.FindByNameAsync(defaultUser) == null)
                {
                    var user = new User { UserName = defaultUser, Email = defaultUser, SecurityStamp = Guid.NewGuid().ToString(), UserFavoriteCurrencies = new List<UserCurrency>() };

                    var result = await userManager.CreateAsync(user, defaultUserPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RolesEnum.User.ToString());

                        Log.Logger.Information($"User {defaultUser} has been created.");
                    }
                }
            }
        }
    }
}
