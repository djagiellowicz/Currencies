using System.Collections;

namespace WalutyBusinessLogic.Models.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public ICollection Roles { get; set; }
    }
}
