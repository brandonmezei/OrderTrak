using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Profile
{
    public class ProfileDTO
    {
        public Guid FormID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
