namespace Common.DTO.UserDTO
{
    public class UserCreateDTO : UserUpdateDTO
    {
        public string Username { get; set; } = null!;
    }
}
