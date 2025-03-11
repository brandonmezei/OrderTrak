using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.StockGroup;
using OrderTrak.API.Services.StockGroup;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.StockGroup
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "StockGroup")]
    public class StockGroupController(IStockGroupService stockGroupService) : ControllerBase
    {
        private readonly IStockGroupService StockGroupService = stockGroupService;

        #region GET
        [HttpGet("GetStockGroup/{stockGroupID}")]
        public async Task<ActionResult<StockGroupDTO>> GetStockGroupAsync(Guid stockGroupID)
        {
            try
            {
                return Ok(await StockGroupService.GetStockGroupAsync(stockGroupID));
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
        [HttpPost("CreateStockGroup")]
        public async Task<ActionResult<Guid>> CreateStockGroupAsync([FromBody] StockGroupCreateDTO stockGroupCreateDTO)
        {
            try
            {
                return Ok(await StockGroupService.CreateStockGroupAsync(stockGroupCreateDTO));
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

        [HttpPost("UpdateStockGroup")]
        public async Task<ActionResult> UpdateStockGroupAsync([FromBody] StockGroupUpdateDTO stockGroupUpdateDTO)
        {
            try
            {
                await StockGroupService.UpdateStockGroupAsync(stockGroupUpdateDTO);
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

        [HttpPost("SearchStockGroup")]
        public async Task<ActionResult<PagedTable<StockGroupSearchReturnDTO>>> SearchStockGroupAsync([FromBody] SearchQueryDTO searchQuery)
        {
            try
            {
                return Ok(await StockGroupService.SearchStockGroupAsync(searchQuery));
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
        [HttpDelete("DeleteStockGroup/{stockGroupID}")]
        public async Task<ActionResult> DeleteStockGroupAsync(Guid stockGroupID)
        {
            try
            {
                await StockGroupService.DeleteStockGroupAsync(stockGroupID);
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
