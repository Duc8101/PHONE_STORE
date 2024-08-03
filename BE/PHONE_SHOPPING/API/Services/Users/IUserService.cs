using Common.Base;
using Common.DTO.UserDTO;
using Common.Entity;

namespace API.Services.Users
{
    public interface IUserService
    {
        ResponseBase Detail(Guid userId);
        ResponseBase Login(LoginDTO DTO);
        Task<ResponseBase> Create(UserCreateDTO DTO);
        Task<ResponseBase> ForgotPassword(ForgotPasswordDTO DTO);
        ResponseBase Update(Guid userId, UserUpdateDTO DTO);
        ResponseBase ChangePassword(Guid userId, ChangePasswordDTO DTO);
        ResponseBase Logout(Guid userId);
        ResponseBase GetUserByToken(string token, string hardware);
    }
}
