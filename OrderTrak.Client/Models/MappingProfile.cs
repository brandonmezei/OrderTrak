using AutoMapper;
using OrderTrak.Client.Pages.Inventory;
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

            CreateMap<PartDTO, PartUpdateDTO>();

            CreateMap<LocationDTO, LocationUpdateDTO>();

            CreateMap<StockGroupDTO, StockGroupUpdateDTO>();

            CreateMap<PoDTO, POUpdateDTO>();
            CreateMap<POLineDTO, POUpdateLineDTO>();

            CreateMap<ReceivingDTO, ReceivingUpdateDTO>();

            CreateMap<OrderHeaderDTO, OrderHeaderUpdateDTO>();
            CreateMap<OrderPartListDTO, OrderPartListUpdate>();
            CreateMap<OrderShipDTO, OrderShipUpdateDTO>();
            CreateMap<OrderActivationDTO, OrderActivationUpdateDTO>();

            CreateMap<InventorySearchReturnDTO, InventoryUpdateLookupDTO>();
            CreateMap<InventorySearchReturnDTO, InventoryUpdateLookupUDFDTO>();
        }
    }
}
