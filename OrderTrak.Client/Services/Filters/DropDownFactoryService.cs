﻿using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Filters
{
    public class DropDownFactoryService(IClient client) : IDropDownFactoryService
    {
        private readonly IClient ApiService = client;

        public async Task<List<DropDownFilterDTO>> GetUnassignedUsers()
        {
            return [.. await ApiService.GetUnassignedUsersDropDownAsync()];
        }

        public async Task<List<DropDownFilterDTO>> GetUOMAsync()
        {
            return [.. await ApiService.GetUOMDropDownAsync()];
        }

        public async Task<List<DropDownFilterDTO>> GetCustomersAsync()
        {
            return [.. await ApiService.GetCustomersDropDownAsync()];
        }

        public async Task<List<DropDownFilterDTO>> GetProjectsAsync(Guid CustomerID)
        {
            return [.. await ApiService.GetProjectsDropDownAsync(CustomerID)];
        }

        public async Task<List<DropDownFilterDTO>> GetStockGroupAsync()
        {
            return [.. await ApiService.GetStockGroupDropDownAsync()];
        }

        public async Task<List<DropDownFilterDTO>> GetPOListGroupAsync(POListFilterDTO pOListFilterDTO)
        {
            return [.. await ApiService.GetPOListGroupDropDownAsync(pOListFilterDTO)];
        }

        public async Task<List<DropDownFilterDTO>> GetOrderStatusListAsync()
        {
            return [.. await ApiService.GetOrderStatusListDropDownAsync()];
        }

        public async Task<List<DropDownFilterDTO>> GetInventoryStatusListAsync()
        {
            return [.. await ApiService.GetInventoryStatusListDropDownAsync()];
        }
    }
}
