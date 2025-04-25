using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Receiving;
using OrderTrak.API.Services.Receiving;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Receiving
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Receiving")]
    public class ReceivingController(IReceivingService receivingService) : ControllerBase
    {
        private readonly IReceivingService ReceivingService = receivingService;

        #region GET
        [HttpGet("GetReceiving/{recID}")]
        public async Task<ActionResult<ReceivingDTO>> GetReceivingAsync(Guid recID)
        {
            try
            {
                return Ok(await ReceivingService.GetReceivingAsync(recID));
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
        [HttpPost("CreateReceiving")]
        public async Task<ActionResult<Guid>> CreateReceivingAsync([FromBody] ReceivingCreateDTO receivingCreateDTO)
        {
            try
            {
                return Ok(await ReceivingService.CreateReceivingAsync(receivingCreateDTO));
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

        [HttpPost("SearchReceiving")]
        public async Task<ActionResult<PagedTable<ReceivingSearchReturnDTO>>> SearchReceivingAsync([FromBody] ReceivingSearchDTO searchQuery)
        {
            try
            {
                return Ok(await ReceivingService.SearchReceivingAsync(searchQuery));
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

        [HttpPost("CreateReceivingLine")]
        public async Task<ActionResult> CreateReceivingLineAsync([FromBody] ReceivingLineCreateDTO receivingLineCreateDTO)
        {
            try
            {
                await ReceivingService.CreateReceivingLineAsync(receivingLineCreateDTO);
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

        [HttpPost("UpdateReceiving")]
        public async Task<ActionResult> UpdateReceivingAsync([FromBody] ReceivingUpdateDTO receivingUpdateDTO)
        {
            try
            {
                await ReceivingService.UpdateReceivingAsync(receivingUpdateDTO);
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
        [HttpDelete("DeleteReceiving/{recID}")]
        public async Task<ActionResult> DeleteReceivingAsync(Guid recID)
        {
            try
            {
                await ReceivingService.DeleteReceivingAsync(recID);
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
