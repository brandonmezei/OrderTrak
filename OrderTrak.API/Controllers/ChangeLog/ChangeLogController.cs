using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Services.ChangeLog;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.ChangeLog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChangeLogController(IChangeLogService changeLogService) : ControllerBase
    {
        private readonly IChangeLogService _changeLogService = changeLogService;

        #region POST
        [HttpPost("GetChangeLogs")]
        public async Task<IActionResult> GetChangeLogsAsync(SearchQueryDTO searchQuery)
        {
            try
            {
                return Ok(await _changeLogService.GetChangeLogsAsync(searchQuery));
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
