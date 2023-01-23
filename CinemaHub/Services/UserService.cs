using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;
using CinemaHub.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaHub.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private IQueryable<ApplicationUser> Query => dbContext.Users
            .AsQueryable();
        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.dbContext = applicationDbContext;
        }

        public async Task<ApplicationUser> CreateUserAsync(NewUserRequest request)
        {
            if (await userManager.Users.AnyAsync(x => x.NormalizedEmail == request.Email.ToUpperInvariant()))
            {
                throw new EntityValidationException("User with that email already exists");
            }
            var applicationUser = mapper.Map<ApplicationUser>(request);
            applicationUser.Status = ApplicationUser.UserStatus.Active;
            applicationUser.UserName = request.Email;
            applicationUser.CreatedOn = DateTime.Now;
            applicationUser.EmailConfirmed = true;

            var result = await userManager.CreateAsync(applicationUser, request.Password);
            if (!result.Succeeded)
            {
                throw new EntityValidationException(result.Errors.FirstOrDefault()?.Description);
            }
            await SetUserRole(applicationUser, request.Role);
            return applicationUser;
        }
        public async Task<ApplicationUser> UpdateAsync(Guid userId, UpdateUserRequest updateUserRequest)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return null;

            await SetUserRole(user, updateUserRequest.Role);

            user.FirstName = updateUserRequest.FirstName ?? user.FirstName;
            user.LastName = updateUserRequest.LastName ?? user.LastName;
            user.PhoneNumber = updateUserRequest.PhoneNumber ?? user.PhoneNumber;

            await userManager.UpdateAsync(user);

            return user;
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity == null) return null;
            var userId = principal.Identity.Name;

            return await Query.Where(x => x.UserName == userId).FirstOrDefaultAsync();
        }

        public async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
           
        }

        public async Task<IEnumerable<ApplicationUser>> SearchAsync(UserTextSearchRequest request)
        {
            var query = ExtractSearchQuery(request);
            return await query.Sorted(request).Paged(request).ToListAsync();
        }

        private IQueryable<ApplicationUser> ExtractSearchQuery(UserTextSearchRequest request)
        {
            var query = Query;
            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(x => x.Email.Contains(request.Email.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(request.SearchText.ToLower()) ||
                    x.LastName.ToLower().Contains(request.SearchText.ToLower()));
            }

            return query;
        }
        public async Task<int> CountAsync(UserTextSearchRequest request)
        {
            var query = ExtractSearchQuery(request);
            return await Query.CountAsync();
        }

        public async Task DeleteAsync(ApplicationUser applicationUser)
        {
            applicationUser.Status = ApplicationUser.UserStatus.Retired;
            await userManager.UpdateAsync(applicationUser);
        }

        public async Task<ApplicationUser> GetUserById(Guid id)
        {
            return await Query.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        private async Task SetUserRole(ApplicationUser user, string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {

                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, userRoles);
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

    }
}
