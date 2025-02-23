using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO.Project;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Project
{
    public class ProjectService(OrderTrakContext orderTrakContext) : IProjectService
    {
        private readonly OrderTrakContext _orderTrakContext = orderTrakContext;

        public async Task<Guid> CreateProjectAsync(ProjectCreateDTO projectCreateDTO)
        {
            // Get Customer
            var customer = await _orderTrakContext.UPL_Customer
                .Include(x => x.UPL_Projects.Where(i => i.ProjectCode == projectCreateDTO.ProjectCode))
                .FirstOrDefaultAsync(x => x.FormID == projectCreateDTO.CustID)
                ?? throw new ValidationException("Customer not found.");

            if (customer.UPL_Projects.Count > 0)
                throw new ValidationException($"Project {projectCreateDTO.ProjectCode} already exists in customer {customer.CustomerCode}.");

            // Create new Project
            var project = new UPL_Project
            {
                ProjectCode = projectCreateDTO.ProjectCode,
                ProjectName = projectCreateDTO.ProjectName,
                ContactName = projectCreateDTO.ContactName,
                ContactPhone = projectCreateDTO.ContactPhone,
                ContactEmail = projectCreateDTO.ContactEmail,
                UDF1 = projectCreateDTO.UDF1,
                UDF2 = projectCreateDTO.UDF2,
                UDF3 = projectCreateDTO.UDF3,
                UDF4 = projectCreateDTO.UDF4,
                UDF5 = projectCreateDTO.UDF5,
                UDF6 = projectCreateDTO.UDF6,
                UDF7 = projectCreateDTO.UDF7,
                UDF8 = projectCreateDTO.UDF8,
                UDF9 = projectCreateDTO.UDF9,
                UDF10 = projectCreateDTO.UDF10
            };

            // Save
            customer.UPL_Projects.Add(project);
            await _orderTrakContext.SaveChangesAsync();

            return project.FormID;
        }

        public async Task UpdateProjectAsync(ProjectUpdateDTO projectUpdateDTO)
        {
            // Get Project
            var project = await _orderTrakContext.UPL_Project
                .FirstOrDefaultAsync(x => x.FormID == projectUpdateDTO.FormID)
                ?? throw new ValidationException("Project not found.");

            // Check if project already exists in this customer
            if (await _orderTrakContext.UPL_Project.AnyAsync(x => x.ProjectCode == projectUpdateDTO.ProjectCode && x.FormID != projectUpdateDTO.FormID && x.CustomerID == project.CustomerID))
                throw new ValidationException($"Project {projectUpdateDTO.ProjectCode} already exists.");

            // Update project
            project.ProjectCode = projectUpdateDTO.ProjectCode;
            project.ProjectName = projectUpdateDTO.ProjectName;
            project.ContactName = projectUpdateDTO.ContactName;
            project.ContactPhone = projectUpdateDTO.ContactPhone;
            project.ContactEmail = projectUpdateDTO.ContactEmail;
            project.UDF1 = projectUpdateDTO.UDF1;
            project.UDF2 = projectUpdateDTO.UDF2;
            project.UDF3 = projectUpdateDTO.UDF3;
            project.UDF4 = projectUpdateDTO.UDF4;
            project.UDF5 = projectUpdateDTO.UDF5;
            project.UDF6 = projectUpdateDTO.UDF6;
            project.UDF7 = projectUpdateDTO.UDF7;
            project.UDF8 = projectUpdateDTO.UDF8;
            project.UDF9 = projectUpdateDTO.UDF9;
            project.UDF10 = projectUpdateDTO.UDF10;

            // Save
            await _orderTrakContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Guid projectID)
        {
            // Get Project
            var project = await _orderTrakContext.UPL_Project
                .FirstOrDefaultAsync(x => x.FormID == projectID)
                ?? throw new ValidationException("Project not found.");

            // Delete
            project.IsDelete = true;

            await _orderTrakContext.SaveChangesAsync();
        }

        public async Task<ProjectDTO> GetProjectAsync(Guid projectID)
        {
            // Get Project
            return await _orderTrakContext.UPL_Project
                .Include(x => x.UPL_Customer)
                .Select(x => new ProjectDTO
                {
                    FormID = x.FormID,
                    CustomerCode = x.UPL_Customer.CustomerCode,
                    CustomerName = x.UPL_Customer.CustomerName,
                    ProjectCode = x.ProjectCode,
                    ProjectName = x.ProjectName,
                    ContactName = x.ContactName,
                    ContactPhone = x.ContactPhone,
                    ContactEmail = x.ContactEmail,
                    UDF1 = x.UDF1,
                    UDF2 = x.UDF2,
                    UDF3 = x.UDF3,
                    UDF4 = x.UDF4,
                    UDF5 = x.UDF5,
                    UDF6 = x.UDF6,
                    UDF7 = x.UDF7,
                    UDF8 = x.UDF8,
                    UDF9 = x.UDF9,
                    UDF10 = x.UDF10
                })
                .FirstOrDefaultAsync(x => x.FormID == projectID)
                ?? throw new ValidationException("Project not found.");
        }
    }
}
