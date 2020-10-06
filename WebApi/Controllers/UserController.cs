using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Enums;
using WebApi.Requests.Users;
using WebApi.Responses.Users;
using WebApi.Services;
using WebApi.Shared;

namespace WebApi.Apis
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ApiBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("{id:guid}/activate")]
        public ActionResult Activate([FromRoute] ActivateUserRequest request)
        {
            _userService.Activate(request);

            return Ok();
        }

        [HttpGet("/api/user/context")]
        public GetContextUserResponse Context()
        {
            var user = _userService.GetContext();

            return new GetContextUserResponse()
            {
                Data = user
            };
        }

        [HttpPatch("{id:guid}/change-password")]
        public ActionResult ChangePassword(Guid id, [FromBody] ChangePasswordUserRequest request)
        {
            request.Id = id;
            _userService.ChangePassword(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPatch("{id:guid}/confirm")]
        public ActionResult Confirm([FromRoute] ConfirmUserRequest request)
        {
            _userService.Confirm(request);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete([FromRoute] DeleteUserRequest request)
        {
            _userService.Delete(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("email-exists/{email}")]
        public EmailExistsUserReponse Get([FromRoute] EmailExistsUserRequest request)
        {
            var email = _userService.EmailExists(request);

            return new EmailExistsUserReponse()
            {
                Data = email
            };
        }

        [HttpGet("{id:guid}")]
        public GetUserResponse Get([FromRoute] GetUserRequest request)
        {
            var user = _userService.GetById(request);

            return new GetUserResponse()
            {
                Data = user
            };
        }

        [HttpGet("types/{type}")]
        public ActionResult<ListUsersResponse> List([FromQuery] ListUsersRequest request)
        {
            var (users, pagination) = _userService.List(request);

            return new ListUsersResponse()
            {
                Data = users,
                Pagination = pagination
            };
        }

        [AllowAnonymous]
        [HttpPost("/api/user/login")]
        public async Task<LoginUserResponse> Login([FromBody] LoginUserRequest request)
        {
            var user = await _userService.Login(request);

            return new LoginUserResponse()
            {
                Data = user
            };
        }

        [HttpPost("/api/user/logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("/api/user/register")]
        public RegisterUserResponse Register([FromBody] RegisterUserRequest request)
        {
            var user = _userService.Register(request);

            return new RegisterUserResponse()
            {
                Data = user
            };
        }

        [HttpPatch("{id:guid}/undelete")]
        public ActionResult Undelete([FromRoute] UndeleteUserRequest request)
        {
             _userService.Undelete(request);

            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<UpdateUserResponse> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userService.Update(id, request);

            return new UpdateUserResponse()
            {
                Data = user
            };
        }

        [HttpPut("/api/user/regions")]
        public GetUserRegionsResponse Regions()
        {
            return new GetUserRegionsResponse()
            {
                Data = new Dictionary<string, string>()
                {
                    { "Prekmurje", "Prekmurje" },
                    { "Štajerska", "Štajerska" },
                    { "Koroška", "Koroška" },
                    { "Gorenjska", "Gorenjska" },
                    { "Severna Primorska", "SevernaPrimorska" },
                    { "Južna Primorska", "JužnaPrimorska" },
                    { "Notranjska", "Notranjska" },
                    { "Dolenjska", "Dolenjska" },
                    { "Osrednja Slovenija", "OsrednjaSlovenija" }
                }
            };
        }
    }
}
