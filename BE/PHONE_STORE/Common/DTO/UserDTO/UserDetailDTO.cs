namespace Common.DTO.UserDTO
{
    public class UserDetailDTO : UserUpdateDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
