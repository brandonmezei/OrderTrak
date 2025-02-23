using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO.Project;
using OrderTrak.API.Services.Project;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Project
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Project")]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        #region GET
        [HttpGet("GetProject/{projectID}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectAsync(Guid projectID)
        {
            try
            {
                return Ok(await _projectService.GetProjectAsync(projectID));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region POST
        [HttpPost("CreateProject")]
        public async Task<ActionResult<Guid>> CreateProjectAsync([FromBody] ProjectCreateDTO projectCreateDTO)
        {
            try
            {
                return Ok(await _projectService.CreateProjectAsync(projectCreateDTO));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("UpdateProject")]
        public async Task<ActionResult> UpdateProjectAsync([FromBody] ProjectUpdateDTO projectUpdateDTO)
        {
            try
            {
                await _projectService.UpdateProjectAsync(projectUpdateDTO);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteProject/{projectID}")]
        public async Task<ActionResult> DeleteProjectAsync(Guid projectID)
        {
            try
            {
                await _projectService.DeleteProjectAsync(projectID);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
