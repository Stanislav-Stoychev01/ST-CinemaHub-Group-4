using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Data;

namespace CinemaHub.Controllers.Model
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ApplicationUser.UserStatus Status { get; set; }
        public string Role { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
