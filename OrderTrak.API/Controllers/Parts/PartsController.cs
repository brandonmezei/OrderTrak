using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Parts;
using OrderTrak.API.Services.Parts;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Parts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Parts")]
    public class PartsController(IPartService partService) : ControllerBase
    {
        private readonly IPartService PartService = partService;

        #region GET
        [HttpGet("GetPart/{partID}")]
        public async Task<ActionResult<PartDTO>> GetPartAsync(Guid partID)
        {
            try
            {
                return Ok(await PartService.GetPartAsync(partID));
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
        [HttpPost("CreatePart")]
        public async Task<ActionResult<Guid>> CreatePartAsync([FromBody] PartCreateDTO partCreateDTO)
        {
            try
            {
                return Ok(await PartService.CreatePartAsync(partCreateDTO));
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

        [HttpPost("UpdatePart")]
        public async Task<ActionResult> UpdatePartAsync([FromBody] PartUpdateDTO partUpdateDTO)
        {
            try
            {
                await PartService.UpdatePartAsync(partUpdateDTO);
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

        [HttpPost("SearchPart")]
        public async Task<ActionResult<PagedTable<PartSearchReturnDTO>>> SearchPartAsync([FromBody] PartSearchDTO searchQuery)
        {
            try
            {
                return Ok(await PartService.SearchPartsAsync(searchQuery));
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
        [HttpDelete("DeletePart/{partID}")]
        public async Task<ActionResult> DeletePartAsync(Guid partID)
        {
            try
            {
                await PartService.DeletePartAsync(partID);
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
