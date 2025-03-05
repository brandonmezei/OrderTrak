using AutoMapper;
using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerDTO, CustomerUpdateDTO>();
            CreateMap<ProjectDTO, ProjectUpdateDTO>();
            CreateMap<ProfileDTO, ProfileUpdateDTO>();
            CreateMap<RoleDTO, RoleUpdateDTO>();
            CreateMap<RoleToFunctionDTO, RoleUpdateRoleToFunctionListDTO>();
            CreateMap<ProfileDTO, UserAdminUpdateDTO>();
        }
    }
}
