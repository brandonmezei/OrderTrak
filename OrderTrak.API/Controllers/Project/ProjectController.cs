using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpGet("get/{projectID}")]
        public async Task<IActionResult> GetProjectAsync(Guid projectID)
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
        [HttpPost("create")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectCreateDTO projectCreateDTO)
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

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProjectAsync([FromBody] ProjectUpdateDTO projectUpdateDTO)
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
        [HttpDelete("delete/{projectID}")]
        public async Task<IActionResult> DeleteProjectAsync(Guid projectID)
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
