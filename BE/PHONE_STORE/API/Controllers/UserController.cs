using API.Attributes;
using API.Services.Users;
using Common.Base;
using Common.DTO.UserDTO;
using Common.Entity;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseAPIController
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ResponseBase Detail()
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            return _service.Detail(Guid.Parse(userId));
        }

        [HttpPost]
        public ResponseBase Login([Required] LoginDTO DTO)
        {
            return _service.Login(DTO);
        }

        [HttpGet]
        [Authorize]
        public ResponseBase Logout()
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase(false, "Not found user id", (int)HttpStatusCode.NotFound);
            }
            return _service.Logout(Guid.Parse(userId));
        }

        [HttpPost]
        public async Task<ResponseBase> Create([Required] UserCreateDTO DTO)
        {
            return await _service.Create(DTO);
        }

        [HttpPost]
        public async Task<ResponseBase> ForgotPassword(ForgotPasswordDTO DTO)
        {
            return await _service.ForgotPassword(DTO);
        }

        [HttpPut]
        [Role(Roles.Customer)]
        [Authorize]
        public ResponseBase Update([Required] UserUpdateDTO DTO)
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user id", (int)HttpStatusCode.NotFound);
            }
            return _service.Update(Guid.Parse(userId), DTO);
        }


        [HttpPut]
        [Authorize]
        public ResponseBase ChangePassword([Required] ChangePasswordDTO DTO)
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user id", (int)HttpStatusCode.NotFound);
            }
            return _service.ChangePassword(Guid.Parse(userId), DTO);
        }

        [HttpGet]
        public ResponseBase GetUserByToken(string token)
        {
            return _service.GetUserByToken(token);
        }

    }
}
