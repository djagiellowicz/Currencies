using System.Collections;
using System.Collections.Generic;

namespace WalutyBusinessLogic.Models.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Id { get; set; }
        public IList<string> Roles { get; set; }
    }
}
