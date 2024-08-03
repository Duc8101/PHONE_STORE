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
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Detail(Guid.Parse(userId));
            } 
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public ResponseBase Login([Required] LoginDTO DTO)
        {
            ResponseBase response = _service.Login(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        [Authorize]
        public ResponseBase Logout()
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase(false, "Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Logout(Guid.Parse(userId));
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseBase> Create([Required] UserCreateDTO DTO)
        {
            ResponseBase response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseBase> ForgotPassword(ForgotPasswordDTO DTO)
        {
            ResponseBase response = await _service.ForgotPassword(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut]
        [Role(Roles.Customer)]
        [Authorize]
        public ResponseBase Update([Required] UserUpdateDTO DTO)
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Update(Guid.Parse(userId), DTO);
            }   
            Response.StatusCode = response.Code;
            return response;
        }


        [HttpPut]
        [Authorize]
        public ResponseBase ChangePassword([Required] ChangePasswordDTO DTO)
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase(false, "Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                 response = _service.ChangePassword(Guid.Parse(userId), DTO);
            }            
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        public ResponseBase GetUserByToken(string token , string hardware)
        {
            ResponseBase response = _service.GetUserByToken(token, hardware);
            Response.StatusCode = response.Code;
            return response;
        }

    }
}
