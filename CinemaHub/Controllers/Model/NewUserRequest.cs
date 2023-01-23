using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class NewUserRequest
    {
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Role { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
