using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Services.Filters;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Filters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilterFactoryController(IDropDownFilterFactory dropDownFilterFactory) : ControllerBase
    {
        private readonly IDropDownFilterFactory DropDownService = dropDownFilterFactory;

        #region GET
        [HttpGet("GetUnassignedUsersDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetUnassignedUsersAsync()
        {
            try
            {
                return Ok(await DropDownService.GetUnassignedUsers());
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
