using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Project
{
    public interface IProjectService
    {
        Task<Guid> CreateProjectAsync(ProjectCreateDTO projectCreateDTO);
        Task UpdateProjectAsync(ProjectUpdateDTO projectUpdateDTO);
        Task DeleteProjectAsync(Guid projectID);
        Task<ProjectDTO> GetProjectAsync(Guid projectID);
    }
}
