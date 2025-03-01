namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int UserCount { get; set; } = 0;
    }
}
