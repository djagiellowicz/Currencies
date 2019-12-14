using Microsoft.AspNetCore.Identity;

namespace WalutyBusinessLogic.Models
{
    public class UpdateUserResult
    {
        public bool IsPasswordUpdated { get; set; }
        public bool AreRolesUpdated { get; set; }

        public UpdateUserResult(bool isPasswordUpdated, bool areRolesUpdated)
        {
            IsPasswordUpdated = isPasswordUpdated;
            AreRolesUpdated = areRolesUpdated;
        }
    }
}
