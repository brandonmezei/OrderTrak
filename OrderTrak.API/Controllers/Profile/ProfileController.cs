using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
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

        [HttpGet("GetUserProfileByID/{FormID}")]
        public async Task<ActionResult<ProfileDTO>> GetUserProfileAsync(Guid FormID)
        {
            try
            {
                return Ok(await ProfileSettings.GetUserProfileAsync(FormID));
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

        [Authorize(Policy = "UserManager")]
        [HttpPost("SearchUserProfile")]
        public async Task<ActionResult<PagedTable<ProfileDTO>>> SearchUserProfileAsync([FromBody] SearchQueryDTO searchQuery)
        {
            try
            {
                return Ok(await ProfileSettings.SearchUserProfileAsync(searchQuery));
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

        [Authorize(Policy = "UserManager")]
        [HttpPost("UpdateUserAdmin")]
        public async Task<ActionResult> UpdateUserAdminAsync([FromBody] UserAdminUpdateDTO userAdminUpdateDTO)
        {
            try
            {
                await ProfileSettings.UpdateUserAdminAsync(userAdminUpdateDTO);
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
        [Authorize(Policy = "UserManager")]
        [HttpDelete("DeleteUserAdmin/{FormID}")]
        public async Task<ActionResult> DeleteUserAdminAsync(Guid FormID)
        {
            try
            {
                await ProfileSettings.DeleteUserAdminAsync(FormID);
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
