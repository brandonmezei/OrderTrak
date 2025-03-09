using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Project;
using OrderTrak.Client.Shared;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Project
{
    public partial class ProjectEditor : OrderTrakBasePage
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IProjectService ProjectService { get; set; } = default!;

        protected ProjectDTO? Project { get; set; }

        protected bool DeleteProject { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Reset Headers
            Layout.ClearMessages();
            Layout.UpdateHeader("Project Admin", "Create and edit projects.");

            try
            {
                // Get Project from API
                Project = await ProjectService.GetProjectAsync(FormID);
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

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Project != null)
                {
                    // Save Project
                    await ProjectService.UpdateProjectAsync(MapperService.Map<ProjectUpdateDTO>(Project));

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

            DeleteProject = !DeleteProject;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Project != null)
                {
                    // Delete Project
                    await ProjectService.DeleteProjectAsync(Project.FormID);

                    Navigation.NavigateTo($"/customer/{Project.CustomerID}?Delete=true");
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
    }
}