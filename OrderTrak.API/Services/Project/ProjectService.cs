using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO.Project;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Project
{
    public class ProjectService(OrderTrakContext orderTrakContext) : IProjectService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreateProjectAsync(ProjectCreateDTO projectCreateDTO)
        {
            // Get Customer
            var customer = await DB.UPL_Customer
                .Include(x => x.UPL_Projects.Where(i => i.ProjectCode == projectCreateDTO.ProjectCode))
                .FirstOrDefaultAsync(x => x.FormID == projectCreateDTO.CustID)
                ?? throw new ValidationException("Customer not found.");

            if (customer.UPL_Projects.Count > 0)
                throw new ValidationException($"Project {projectCreateDTO.ProjectCode} already exists in customer {customer.CustomerCode}.");

            // Check if customer has 50 projects
            if (await DB.UPL_Project.CountAsync(x => x.FormID == projectCreateDTO.CustID) >= 50)
                throw new ValidationException("Customers have a 50 project max.");

            // Create new Project
            var project = new UPL_Project
            {
                ProjectCode = projectCreateDTO.ProjectCode ?? throw new ValidationException("Project Code is required."),
                ProjectName = projectCreateDTO.ProjectName ?? throw new ValidationException("Project Name is required."),
                ContactName = projectCreateDTO.ContactName ?? throw new ValidationException("Contact Name is required."),
                ContactPhone = projectCreateDTO.ContactPhone ?? throw new ValidationException("Contact Phone is required.")
            };

            // Save
            customer.UPL_Projects.Add(project);
            await DB.SaveChangesAsync();

            return project.FormID;
        }

        public async Task UpdateProjectAsync(ProjectUpdateDTO projectUpdateDTO)
        {
            // Get Project
            var project = await DB.UPL_Project
                .FirstOrDefaultAsync(x => x.FormID == projectUpdateDTO.FormID)
                ?? throw new ValidationException("Project not found.");

            // Check if project already exists in this customer
            if (await DB.UPL_Project.AnyAsync(x => x.ProjectCode == projectUpdateDTO.ProjectCode && x.FormID != projectUpdateDTO.FormID && x.CustomerID == project.CustomerID))
                throw new ValidationException($"Project {projectUpdateDTO.ProjectCode} already exists.");

            // Update project
            project.ProjectCode = projectUpdateDTO.ProjectCode ?? throw new ValidationException("Project Code is required.");
            project.ProjectName = projectUpdateDTO.ProjectName ?? throw new ValidationException("Project Name is required.");
            project.ContactName = projectUpdateDTO.ContactName ?? throw new ValidationException("Contact Name is required.");
            project.ContactPhone = projectUpdateDTO.ContactPhone ?? throw new ValidationException("Contact Phone is required.");
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
            project.StakeHolderEmail = projectUpdateDTO.StakeHolderEmail;
            project.OrderUDF1 = projectUpdateDTO.OrderUDF1;
            project.OrderUDF2 = projectUpdateDTO.OrderUDF2;
            project.OrderUDF3 = projectUpdateDTO.OrderUDF3;
            project.OrderUDF4 = projectUpdateDTO.OrderUDF4;
            project.OrderUDF5 = projectUpdateDTO.OrderUDF5;
            project.OrderUDF6 = projectUpdateDTO.OrderUDF6;
            project.OrderUDF7 = projectUpdateDTO.OrderUDF7;
            project.OrderUDF8 = projectUpdateDTO.OrderUDF8;
            project.OrderUDF9 = projectUpdateDTO.OrderUDF9;
            project.OrderUDF10 = projectUpdateDTO.OrderUDF10;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Guid projectID)
        {
            // Get Project
            var project = await DB.UPL_Project
                .FirstOrDefaultAsync(x => x.FormID == projectID)
                ?? throw new ValidationException("Project not found.");

            // Delete
            project.IsDelete = true;

            await DB.SaveChangesAsync();
        }

        public async Task<ProjectDTO> GetProjectAsync(Guid projectID)
        {
            // Get Project
            return await DB.UPL_Project
                .Include(x => x.UPL_Customer)
                 .AsNoTracking()
                .Select(x => new ProjectDTO
                {
                    FormID = x.FormID,
                    CustomerID = x.UPL_Customer.FormID,
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
                    UDF10 = x.UDF10,
                    StakeHolderEmail = x.StakeHolderEmail,
                    OrderUDF1 = x.OrderUDF1,
                    OrderUDF2 = x.OrderUDF2,
                    OrderUDF3 = x.OrderUDF3,
                    OrderUDF4 = x.OrderUDF4,
                    OrderUDF5 = x.OrderUDF5,
                    OrderUDF6 = x.OrderUDF6,
                    OrderUDF7 = x.OrderUDF7,
                    OrderUDF8 = x.OrderUDF8,
                    OrderUDF9 = x.OrderUDF9,
                    OrderUDF10 = x.OrderUDF10
                })
                .FirstOrDefaultAsync(x => x.FormID == projectID)
                ?? throw new ValidationException("Project not found.");
        }

        public async Task<List<CustomerProjectListDTO>> GetProjectListByCustomerID(Guid customerID)
        {
            return await DB.UPL_Project
                .Where(x => x.UPL_Customer.FormID == customerID)
                .OrderBy(x => x.ProjectCode)
                 .AsNoTracking()
                .Select(x => new CustomerProjectListDTO
                {
                    FormID = x.FormID,
                    ProjectCode = x.ProjectCode,
                    ProjectName = x.ProjectName
                })
                .ToListAsync();
        }
    }
}
