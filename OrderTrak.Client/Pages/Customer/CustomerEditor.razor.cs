using AutoMapper;
using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Customer;
using OrderTrak.Client.Services.Project;
using OrderTrak.Client.Shared;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Customer
{
    public partial class CustomerEditor : OrderTrakBasePage
    {
        [Parameter]
        public Guid FormID { get; set; }

        [SupplyParameterFromQuery]
        public bool Delete { get; set; }

        [Inject]
        private ICustomerService CustomerService { get; set; } = default!;

        [Inject]
        private IProjectService ProjectService { get; set; } = default!;

        protected CustomerDTO? Customer { get; set; }

        protected ProjectCreateDTO? CreateProject { get; set; }

        protected List<CustomerProjectListDTO>? FilteredProjectList { get; set; }
        protected List<CustomerProjectListDTO>? ProjectListFromDB { get; set; }

        protected TableSearch ProjectSearchFilter { get; set; } = new();

        protected int SortOrder { get; set; }

        protected bool CanEditProjects { get; set; }
        protected bool DeleteCustomer { get; set; }

        protected Guid? DeleteProjectID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Customer Admin", "Create and edit customers. Add projects to customers.");

            if (Delete)
                Layout.AddMessage(Messages.DeleteSuccesful, MessageType.Success);

            try
            {
                var permission = await LocalStorage.GetItemAsync<List<string>>("permissions") ?? [];

                CanEditProjects = permission.Contains("Project");

                Customer = await CustomerService.GetCustomerAsync(FormID);
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
                IsCardLoading = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    // Sleep for 500ms to allow the page to render before loading the data
                    await Task.Delay(500);

                    ProjectListFromDB = await ProjectService.GetProjectListByCustomerID(FormID);
                    FilteredProjectList = ProjectListFromDB;
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

        protected async Task ProjectSearch_Click()
        {
            Layout.ClearMessages();

            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (Customer != null)
                {
                    ProjectListFromDB = await ProjectService.GetProjectListByCustomerID(Customer.FormID);
                    FilteredProjectList = ProjectListFromDB;

                    if (!string.IsNullOrEmpty(ProjectSearchFilter.SearchText))
                        FilteredProjectList = [.. ProjectListFromDB
                        .Where(
                            p => p.ProjectName.Contains(ProjectSearchFilter.SearchText, StringComparison.OrdinalIgnoreCase) || p.ProjectCode.Contains(ProjectSearchFilter.SearchText, StringComparison.OrdinalIgnoreCase)
                        )
                        ];
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
                StateHasChanged();
            }
        }

        protected void SortSwitch_Click(int column)
        {
            Layout.ClearMessages();

            if (FilteredProjectList != null)
            {
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
        }

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Customer != null)
                {
                    await CustomerService.UpdateCustomerAsync(MapperService.Map<CustomerUpdateDTO>(Customer));

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

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();
            try
            {
                if (Customer != null)
                {
                    await CustomerService.DeleteCustomerAsync(FormID);

                    Navigation.NavigateTo($"/customer/search?Delete=true");
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

        protected void DeleteProject_Click(Guid? projectID)
        {
            Layout.ClearMessages();

            DeleteProjectID = projectID;
        }

        protected async Task DeleteProjectConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (DeleteProjectID != null)
                {
                    await ProjectService.DeleteProjectAsync(DeleteProjectID.Value);
                    Customer = await CustomerService.GetCustomerAsync(FormID);

                    DeleteProjectID = null;

                    ProjectListFromDB = await ProjectService.GetProjectListByCustomerID(Customer.FormID);
                    FilteredProjectList = ProjectListFromDB;

                    Layout.AddMessage(Messages.DeleteSuccesful, MessageType.Success);
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

        protected void AddProject_Toggle()
        {
            Layout.ClearMessages();

            if(CreateProject == null)
                CreateProject = new ProjectCreateDTO();
            else
                CreateProject = null;
        }

        protected async Task AddProject_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (Customer != null && CreateProject != null)
                {
                    CreateProject.CustID = Customer.FormID;
                    await ProjectService.CreateProjectAsync(CreateProject);

                    // Reload Project List
                    ProjectListFromDB = await ProjectService.GetProjectListByCustomerID(Customer.FormID);
                    FilteredProjectList = ProjectListFromDB;

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
                CreateProject = null;
                IsLoading = false;
            }
        }
    }
}