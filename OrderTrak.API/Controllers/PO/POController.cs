using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.PO;
using OrderTrak.API.Services.PO;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.PO
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "PurchaseOrder")]

    public class POController(IPOService pOService) : ControllerBase
    {
        private readonly IPOService pOService = pOService;

        #region GET
        [HttpGet("GetPO/{poID}")]
        public async Task<ActionResult<PoDTO>> GetPOAsync(Guid poID)
        {
            try
            {
                return Ok(await pOService.GetPOAsync(poID));
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
        [HttpPost("CreatePO")]
        public async Task<ActionResult<Guid>> CreatePOAsync([FromBody] POCreateDTO pOCreateDTO)
        {
            try
            {
                return Ok(await pOService.CreatePOAsync(pOCreateDTO));
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

        [HttpPost("UpdatePO")]
        public async Task<ActionResult> UpdatePOAsync([FromBody] POUpdateDTO pOUpdateDTO)
        {
            try
            {
                await pOService.UpdatePOAsync(pOUpdateDTO);
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

        [HttpPost("SearchPO")]
        public async Task<ActionResult<PagedTable<POSearchReturnDTO>>> SearchPOAsync([FromBody] POSearchDTO searchQuery)
        {
            try
            {
                return Ok(await pOService.SearchPOAsync(searchQuery));
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

        [HttpPost("CreatePOLine")]
        public async Task<ActionResult> CreatePOLine([FromBody] POCreateLineDTO poLineCreateDTO)
        {
            try
            {
                await pOService.CreatePOLineAsync(poLineCreateDTO);
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

        [HttpPost("UpdatePOLine")]
        public async Task<ActionResult> UpdatePOLine([FromBody] POUpdateLineDTO poLineUpdateDTO)
        {
            try
            {
                await pOService.UpdatePOLineAsync(poLineUpdateDTO);
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
        [HttpDelete("DeletePO/{poID}")]
        public async Task<ActionResult> DeletePOAsync(Guid poID)
        {
            try
            {
                await pOService.DeletePOAsync(poID);
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

        [HttpDelete("DeletePOLine/{FormID}")]
        public async Task<ActionResult> DeletePOLineAsync(Guid FormID)
        {
            try
            {
                await pOService.DeletePOLineAsync(FormID);
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
