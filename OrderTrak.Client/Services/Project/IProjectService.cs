using OrderTrak.Client.Services.API;
using System.Threading.Tasks;

namespace OrderTrak.Client.Services.Project
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
