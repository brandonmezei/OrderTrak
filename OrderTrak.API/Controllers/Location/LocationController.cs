using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Location;
using OrderTrak.API.Services.Location;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Location")]
    public class LocationController(ILocationService locationService) : ControllerBase
    {
        private readonly ILocationService LocationService = locationService;

        #region GET
        [HttpGet("GetLocation/{locationId}")]
        public async Task<ActionResult<LocationDTO>> GetLocationAsync(Guid locationId)
        {
            try
            {
                return Ok(await LocationService.GetLocationAsync(locationId));
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
        [HttpPost("CreateLocation")]
        public async Task<ActionResult<Guid>> CreateLocationAsync([FromBody] LocationCreateDTO locationCreateDTO)
        {
            try
            {
                return Ok(await LocationService.CreateLocationAsync(locationCreateDTO));
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

        [HttpPost("UpdateLocation")]
        public async Task<ActionResult> UpdateLocationAsync([FromBody] LocationUpdateDTO locationUpdateDTO)
        {
            try
            {
                await LocationService.UpdateLocationAsync(locationUpdateDTO);
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

        [HttpPost("SearchLocation")]
        public async Task<ActionResult<PagedTable<LocationSearchReturnDTO>>> SearchLocationAsync([FromBody] SearchQueryDTO searchQuery)
        {
            try
            {
                return Ok(await LocationService.SearchLocationAsync(searchQuery));
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
        [HttpDelete("DeleteLocation/{locationId}")]
        public async Task<ActionResult> DeleteLocationAsync(Guid locationId)
        {
            try
            {
                await LocationService.DeleteLocationAsync(locationId);
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
