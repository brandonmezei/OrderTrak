using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.ChangeLog;
using OrderTrak.API.Services.ChangeLog;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.ChangeLog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChangeLogController(IChangeLogService changeLogService) : ControllerBase
    {
        private readonly IChangeLogService ChangeLogService = changeLogService;

        #region POST
        [HttpPost("GetChangeLogs")]
        public async Task<ActionResult<PagedTable<ChangeLogDTO>>> GetChangeLogsAsync(SearchQueryDTO searchQuery)
        {
            try
            {
                return Ok(await ChangeLogService.GetChangeLogsAsync(searchQuery));
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
