using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;
using CinemaHub.Services;
using CinemaHub.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaHub.Controllers
{
    [Route("api/admin")]
    [Authorize(policy: "AdminOnly")]
    public class UserAdminController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public UserAdminController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet("users")]
        [SwaggerResponse(200, "Success", typeof(PaginatedResponse<UserResponse>))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserTextSearchRequest request)
        {
            var totalCount = await userService.CountAsync(request);
            var searchResults = await userService.SearchAsync(request);

            var users = searchResults.Select(x => mapper.Map<UserResponse>(x)).ToList();
            foreach (var user in users)
            {
                var applicationUser = searchResults.FirstOrDefault(x => x.Id == user.Id);
                user.Role = await userService.GetUserRoleAsync(applicationUser);

            }
            var response = new PaginatedResponse<UserResponse>(users,
                request?.StartAt ?? 0,
                totalCount,
                request?.SortFields);

            return Ok(response);
        }

        [HttpPut("users/{id}")]
        [SwaggerResponse(200, "Success", typeof(UserResponse))]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await userService.GetUserById(id);
                if (user == null)
                    return BadRequest(new ValidationFailedResponse(new ValidationError("id", "User not found")));

                user = await userService.UpdateAsync(id, request);
                var response = mapper.Map<UserResponse>(user);
                return Ok(response);
            }
            catch (EntityValidationException e)
            {
                return BadRequest(new ValidationFailedResponse(new ValidationError("", e.Message)));
            }
            
           
        }

        [HttpDelete("users/{id}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad request", typeof(ValidationFailedResponse))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await userService.GetUserById(id);
                if (user == null)
                    return BadRequest(new ValidationFailedResponse(new ValidationError("id", "User not found")));
                await userService.DeleteAsync(user);
                return Ok();
            }
            catch (EntityValidationException e)
            {
                return BadRequest(new ValidationFailedResponse(new ValidationError("", e.Message)));
            }


        }
    }
}
