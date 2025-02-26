using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Project;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Project
{
    public partial class ProjectEditor
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IProjectService ProjectService { get; set; } = default!;

        protected ProjectDTO? Project { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Project Admin", "Create and edit projects.");

            try
            {
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
    }
}