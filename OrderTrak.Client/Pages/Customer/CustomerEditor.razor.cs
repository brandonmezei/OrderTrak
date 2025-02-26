using AutoMapper;
using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Customer;
using OrderTrak.Client.Shared;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Customer
{
    public partial class CustomerEditor : OrderTrakBasePage
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private ICustomerService CustomerService { get; set; } = default!;

        protected CustomerDTO? Customer { get; set; }

        protected List<CustomerProjectListDTO> FilteredProjectList { get; set; } = [];

        protected TableSearch ProjectSearchFilter { get; set; } = new();

        protected int SortOrder { get; set; }

        protected bool CanEditProjects { get; set; }
        protected bool DeleteCustomer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Customer Admin", "Create and edit customers. Add projects to customers.");

            try
            {
                var permission = await LocalStorage.GetItemAsync<List<string>>("permissions") ?? [];

                CanEditProjects = permission.Contains("Project");

                Customer = await CustomerService.GetCustomerAsync(FormID);
                FilteredProjectList = Customer.ProjectList.ToList();
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

        protected void ProjectSearch_Click()
        {
            Layout.ClearMessages();

            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            if (Customer == null)
            {
                FilteredProjectList = [];
                return;
            }

            if (string.IsNullOrEmpty(ProjectSearchFilter.SearchText))
            {
                FilteredProjectList = [.. Customer.ProjectList];
            }
            else
            {
                FilteredProjectList = [.. Customer.ProjectList
                    .Where(
                        p => p.ProjectName.Contains(ProjectSearchFilter.SearchText, StringComparison.OrdinalIgnoreCase) || p.ProjectCode.Contains(ProjectSearchFilter.SearchText, StringComparison.OrdinalIgnoreCase)
                    )
                ];
            }

            IsLoading = false;
        }

        protected void SortSwitch_Click(int column)
        {
            Layout.ClearMessages();

            if (FilteredProjectList.Count == 0)
                return;

            SortOrder = SortOrder == 1 ? 2 : 1;

            switch (SortOrder)
            {
                case 1:
                    FilteredProjectList = [.. FilteredProjectList
                        .OrderBy(x => column switch
                        {
                            0 => x.ProjectName,
                            1 => x.ProjectCode,
                            _ => x.ProjectName
                        })
                    ];
                    break;

                case 2:
                    FilteredProjectList = [.. FilteredProjectList
                        .OrderByDescending(x => column switch
                        {
                            0 => x.ProjectName,
                            1 => x.ProjectCode,
                            _ => x.ProjectName
                        })
                    ];
                    break;
            }
        }

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Customer != null)
                {
                    await CustomerService.UpdateCustomerAsync(MapperService.Map<CustomerUpdateDTO>(Customer));

                    Customer = await CustomerService.GetCustomerAsync(FormID);
                    ProjectSearch_Click();

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
        }

        protected void Delete_Toggle()
        {
            Layout.ClearMessages();
            DeleteCustomer = !DeleteCustomer;
        }
    }
}