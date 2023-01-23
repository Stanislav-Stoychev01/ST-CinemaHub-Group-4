using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CinemaHub.Areas.Identity.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public string ReturnUrl { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/";
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(Input.Email) ?? await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "You have provided invalid login details. Please check your email/phone number and password.");
                    return Page();
                }
                var result =  await signInManager.PasswordSignInAsync(user, Input.Password, false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(15)
                    });
                    await userManager.ResetAccessFailedCountAsync(user);
                    return Redirect(ReturnUrl);
                }
                ModelState.AddModelError(string.Empty, "You have provided invalid login details. Please check your email/phone number and password.");
                return Page();

            }
            return Page();

        }
    }
}
