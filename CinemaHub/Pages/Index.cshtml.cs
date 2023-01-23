using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Services;

namespace CinemaHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService userService;

        public IndexModel(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated) return Page();
            
            var user = await userService.GetUserAsync(User);
            var role = await userService.GetUserRoleAsync(user);
            if (role == "Admin")
            {
                return Redirect("~/Admin/Dashboard");
            }

            return Page();
        }
    }
}
