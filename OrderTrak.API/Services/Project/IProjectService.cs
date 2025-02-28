using OrderTrak.API.Models.DTO.Project;

namespace OrderTrak.API.Services.Project
{
    public interface IProjectService
    {
        Task<Guid> CreateProjectAsync(ProjectCreateDTO projectCreateDTO);
        Task UpdateProjectAsync(ProjectUpdateDTO projectUpdateDTO);
        Task DeleteProjectAsync(Guid projectID);
        Task<ProjectDTO> GetProjectAsync(Guid projectID);
        Task<List<CustomerProjectListDTO>> GetProjectListByCustomerID(Guid customerID);
    }
}
