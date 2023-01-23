using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;

namespace CinemaHub.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal);
        Task<ApplicationUser> CreateUserAsync(NewUserRequest request);
        Task<ApplicationUser> UpdateAsync(Guid userId, UpdateUserRequest updateUserRequest);
        Task<string> GetUserRoleAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> SearchAsync(UserTextSearchRequest request);
        Task<int> CountAsync(UserTextSearchRequest request);
        Task DeleteAsync(ApplicationUser applicationUser);
        Task<ApplicationUser> GetUserById(Guid id);
    }
}
