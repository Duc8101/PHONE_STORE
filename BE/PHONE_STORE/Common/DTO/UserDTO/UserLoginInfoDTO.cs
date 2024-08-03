namespace Common.DTO.UserDTO
{
    public class UserLoginInfoDTO
    {
        public string Access_Token { get; set; } = string.Empty;
        public Guid UserId {  get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime ExpireDate { get; set; }
    }
}
