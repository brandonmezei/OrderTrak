using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Inventory;
using OrderTrak.API.Services.Inventory;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "InventoryLookup")]
    public class InventoryController(IInventoryService inventoryService) : ControllerBase
    {
        private readonly IInventoryService inventoryService = inventoryService;

        #region POST
        [HttpPost("SearchInventory")]
        public async Task<ActionResult<PagedTable<InventorySearchReturnDTO>>> SearchInventoryAsync([FromBody] InventorySearchDTO searchQuery)
        {
            try
            {
                return Ok(await inventoryService.SearchInventoryAsync(searchQuery));
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
