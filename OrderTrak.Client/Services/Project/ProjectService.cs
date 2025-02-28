using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Project
{
    public class ProjectService(IClient client) : IProjectService
    {
        private readonly IClient ApiService = client;

        public async Task<Guid> CreateProjectAsync(ProjectCreateDTO projectCreateDTO)
        {
            return await ApiService.CreateProjectAsync(projectCreateDTO);
        }

        public async Task DeleteProjectAsync(Guid projectID)
        {
            await ApiService.DeleteProjectAsync(projectID);
        }

        public async Task<ProjectDTO> GetProjectAsync(Guid projectID)
        {
            return await ApiService.GetProjectAsync(projectID);
        }

        public async Task UpdateProjectAsync(ProjectUpdateDTO projectUpdateDTO)
        {
            await ApiService.UpdateProjectAsync(projectUpdateDTO);
        }

        public async Task<List<CustomerProjectListDTO>> GetProjectListByCustomerID(Guid customerID)
        {
            return [.. await ApiService.GetProjectListByCustomerIDAsync(customerID)];
        }
    }
}
