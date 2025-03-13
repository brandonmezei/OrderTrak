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

        protected int SortOrder { get; set; } = 1;

        protected bool CanEditProjects { get; set; }
        protected bool DeleteCustomer { get; set; }

        protected Guid? DeleteProjectID { get; set; }

        protected int SortColumn { get; set; } = 1;

        protected override async Task OnInitializedAsync()
        {
            // Reset Headers
            Layout.ClearMessages();
            Layout.UpdateHeader("Customer Admin", "Create and edit customers. Add projects to customers.");

            // Delete Message
            if (Delete)
                Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);

            try
            {
                // Check Project Permissions
                var permission = await LocalStorage.GetItemAsync<List<string>>("permissions") ?? [];

                CanEditProjects = permission.Contains("Project");

                // Load Customer Data
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

                    // Get Project Info if Can Edit
                    if (CanEditProjects)
                    {
                        ProjectListFromDB = await ProjectService.GetProjectListByCustomerID(FormID);
                        FilteredProjectList = ProjectListFromDB;
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
                    IsCardLoading = false;
                    StateHasChanged();
                }
            }
        }

        protected void ProjectSearch_Click()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            try
            {
                if (Customer != null)
                {

                    // Get Project by filter
                    FilteredProjectList = ProjectListFromDB;

                    if (!string.IsNullOrEmpty(ProjectSearchFilter.SearchText) && FilteredProjectList != null)
                    {
                        var searchFilter = ProjectSearchFilter.SearchText
                           .Split(',')
                           .Select(x => x.Trim())
                           .Where(x => !string.IsNullOrEmpty(x))
                           .ToList();

                        var query = FilteredProjectList
                            .AsQueryable();

                        foreach (var filter in searchFilter)
                        {
                            query = query.Where(
                                 x => x.ProjectName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                    || x.ProjectCode.Contains(filter, StringComparison.OrdinalIgnoreCase)
                            );
                        }

                        FilteredProjectList = [.. query];
                    }
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

            SortColumn = column;

            if (FilteredProjectList != null)
            {
                // Sort Locally No API

                if (FilteredProjectList.Count == 0)
                    return;

                SortOrder = SortOrder == 1 ? 2 : 1;

                switch (SortOrder)
                {
                    case 1:
                        FilteredProjectList = [.. FilteredProjectList
                        .OrderBy(x => column switch
                        {
                            1 => x.ProjectName,
                            2 => x.ProjectCode,
                            _ => x.ProjectName
                        })
                        ];
                        break;

                    case 2:
                        FilteredProjectList = [.. FilteredProjectList
                        .OrderByDescending(x => column switch
                        {
                            1 => x.ProjectName,
                            2 => x.ProjectCode,
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

                    // Save Upper Level Info
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
                    // Delete Customer
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
            finally
            {
                DeleteCustomer = false;
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
                    // Delete Project
                    await ProjectService.DeleteProjectAsync(DeleteProjectID.Value);
                    Customer = await CustomerService.GetCustomerAsync(FormID);

                    // Reload from API
                    ProjectListFromDB = await ProjectService.GetProjectListByCustomerID(Customer.FormID);
                    FilteredProjectList = ProjectListFromDB;
                    ProjectSearch_Click();


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
                DeleteProjectID = null;
            }
        }

        protected void AddProject_Toggle()
        {
            Layout.ClearMessages();

            if (CreateProject == null)
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
                    // Add Project
                    CreateProject.CustID = Customer.FormID;
                    await ProjectService.CreateProjectAsync(CreateProject);

                    // Reload Project List
                    ProjectListFromDB = await ProjectService.GetProjectListByCustomerID(Customer.FormID);
                    FilteredProjectList = ProjectListFromDB;
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
            finally
            {
                CreateProject = null;
                IsLoading = false;
            }
        }
    }
}