namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleToFunctionDTO
    {
        public Guid FormID { get; set; }

        public string FunctionName { get; set; } = string.Empty;

        public bool CanAccess { get; set; }
    }
}
