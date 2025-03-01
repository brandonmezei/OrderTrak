using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO.Profile;
using OrderTrak.API.Services.Profile;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Profile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {
        private readonly IProfileService ProfileSettings = profileService;

        #region GET
        [HttpGet("GetUserProfile")]
        public async Task<ActionResult<ProfileDTO>> GetUserProfileAsync()
        {
            try
            {
                return Ok(await ProfileSettings.GetUserProfileAsync());
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
        [HttpPost("UpdateProfile")]
        public async Task<ActionResult> UpdateProfileAsync([FromBody] ProfileUpdateDTO profileUpdateDTO)
        {
            try
            {
                await ProfileSettings.UpdateProfileAsync(profileUpdateDTO);
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
