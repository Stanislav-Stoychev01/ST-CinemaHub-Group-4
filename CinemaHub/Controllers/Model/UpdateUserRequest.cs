using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Data;

namespace CinemaHub.Controllers.Model
{
    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public ApplicationUser.UserStatus Status { get; set; }
    }
}
