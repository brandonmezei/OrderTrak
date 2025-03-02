using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Roles;
using OrderTrak.API.Services.Roles;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Roles
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Role")]
    public class RolesController(IRoleServices roleServices) : ControllerBase
    {
        private readonly IRoleServices RoleServices = roleServices;

        #region GET
        [HttpGet("GetRole/{roleID}")]
        public async Task<ActionResult<RoleDTO>> GetRoleAsync(Guid roleID)
        {
            try
            {
                return Ok(await RoleServices.GetRoleAsync(roleID));
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

        [HttpGet("GetRoleToFunctionByRoleID/{roleID}")]
        public async Task<ActionResult<List<RoleToFunctionDTO>>> GetRoleToFunctionByRoleIDAsync(Guid roleID)
        {
            try
            {
                return Ok(await RoleServices.GetRoleToFunctionByRoleIDAsync(roleID));
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
        [HttpPost("CreateRole")]
        public async Task<ActionResult<Guid>> CreateRoleAsync([FromBody] RoleCreateDTO roleCreateDTO)
        {
            try
            {
                return Ok(await RoleServices.CreateRoleAsync(roleCreateDTO));
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

        [HttpPost("UpdateRole")]
        public async Task<ActionResult> UpdateRoleAsync([FromBody] RoleUpdateDTO roleUpdateDTO)
        {
            try
            {
                await RoleServices.UpdateRoleAsync(roleUpdateDTO);
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

        [HttpPost("SearchRoles")]
        public async Task<ActionResult<PagedTable<RoleSearchReturnDTO>>> SearchRolesAsync([FromBody] RoleSearchDTO roleSearchDTO)
        {
            try
            {
                return Ok(await RoleServices.SearchRolesAsync(roleSearchDTO));
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

        [HttpPost("UpdateRoleToFunction")]
        public async Task<ActionResult> UpdateRoleToFunctionAsync([FromBody] RoleUpdateRoleToFunctionDTO roleToFunctionUpdateDTO)
        {
            try
            {
                await RoleServices.UpdateRoleToFunctionAsync(roleToFunctionUpdateDTO);
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
        [HttpDelete("DeleteRole/{roleID}")]
        public async Task<ActionResult> DeleteRoleAsync(Guid roleID)
        {
            try
            {
                await RoleServices.DeleteRoleAsync(roleID);
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
