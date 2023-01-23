using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;
using CinemaHub.Services;
using CinemaHub.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaHub.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;
        public RegisterModel(SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            this.signInManager = signInManager;
            this.userService = userService;
        }
        public string ReturnUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } 
        
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(25)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid) return Page();
            
            var role = "User";
            if (Application.Switches.AllowAdminRegister)
                role = "Admin";

            var request = new NewUserRequest()
            {
                Email = Input.Email?.Trim(),
                FirstName = Input.FirstName?.Trim(),
                LastName = Input.LastName?.Trim(),
                Password = Input.Password,
                PhoneNumber = Input.PhoneNumber?.Trim(),
                Role = role
            };
            try
            {
                var user = await userService.CreateUserAsync(request);
                await signInManager.SignInAsync(user, new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(15)
                });
                return Redirect(ReturnUrl);
            }
            catch (EntityValidationException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return Page();
        }

    }
}
