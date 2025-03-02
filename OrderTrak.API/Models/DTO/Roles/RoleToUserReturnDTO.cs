namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleToUserReturnDTO
    {
        public Guid FormID { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
