using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Customer;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Customer
{
    public partial class CustomerSearch
    {
        [Inject]
        private ICustomerService CustomerService { get; set; } = default!;

        [SupplyParameterFromQuery]
        public bool Delete { get; set; }

        protected CustomerSearchDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected CustomerCreateDTO? CreateCustomer { get; set; }


        protected PagedTableOfCustomerSearchReturnDTO? ReturnTable;

        protected Guid? DeleteID { get; set; }

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Customer Admin", "Create and edit customers. Add projects to customers.");

            if (Delete)
                Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);

            IsCardLoading = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                try
                {
                    // Sleep for 500ms to allow the page to render before loading the data
                    await Task.Delay(500);


                    // Get Customers from API
                    SearchFilters = await LocalStorage.GetItemAsync<CustomerSearchDTO>("search") ?? SearchFilters;
                    ReturnTable = await CustomerService.SearchCustomersAsync(SearchFilters);                 
                }
                catch (ApiException ex)
                {
                    Layout.AddMessage(ex.Response, MessageType.Error);
                }
                catch (Exception ex)
                {
                    Layout.AddMessage(ex.Message, MessageType.Error);
                }
                finally
                {
                    IsCardLoading = false;
                    StateHasChanged();
                }
            }
        }

        protected async Task Search_Click()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                SearchFilters.Page = 1;

                // Save Filters
                await LocalStorage.SetItemAsync("search", SearchFilters);

                // Get Customer from API
                ReturnTable = await CustomerService.SearchCustomersAsync(SearchFilters);

                if (ReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task SortSwitch_Click(int column)
        {
            SearchFilters.SortColumn = column;
            SearchFilters.SortOrder = SearchFilters.SortOrder == 1 ? 2 : 1;

            try
            {
                // Save Filters
                await LocalStorage.SetItemAsync("search", SearchFilters);
                
                // Get Customer from API
                ReturnTable = await CustomerService.SearchCustomersAsync(SearchFilters);

                if (ReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
        }

        protected async Task PageSwitch_Click(int page)
        {
            SearchFilters.Page = page;

            try
            {
                // Save Filters
                await LocalStorage.SetItemAsync("search", SearchFilters);

                // Get Customer from API
                ReturnTable = await CustomerService.SearchCustomersAsync(SearchFilters);

                if (ReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
        }

        protected async Task EmptyCustomer_Change()
        {
            SearchFilters.EmptyOnly = !SearchFilters.EmptyOnly;
            await Search_Click();
        }

        protected void CreateCustomer_Toggle()
        {
            if (CreateCustomer == null)
                CreateCustomer = new();
            else
                CreateCustomer = null;
        }

        protected async Task CreateCustomer_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if(CreateCustomer != null)
                {
                    // Create the Customer
                    await CustomerService.CreateCustomerAsync(CreateCustomer);

                    // Reload Customer List
                    ReturnTable = await CustomerService.SearchCustomersAsync(SearchFilters);
                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                CreateCustomer = null;
                IsLoading = false;
            }
        }

        protected void Delete_Toggle(Guid? FormID)
        {
            Layout.ClearMessages();

            DeleteID = FormID;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (DeleteID.HasValue)
                {
                    // Delete the Customer
                    await CustomerService.DeleteCustomerAsync(DeleteID.Value);

                    // Reload Customer List
                    ReturnTable = await CustomerService.SearchCustomersAsync(SearchFilters);
                    Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                DeleteID = null;
            }
        }
    }
}