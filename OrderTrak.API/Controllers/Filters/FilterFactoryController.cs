﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Filters;
using OrderTrak.API.Services.Filters;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Filters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilterFactoryController(IDropDownFilterFactory dropDownFilterFactory) : ControllerBase
    {
        private readonly IDropDownFilterFactory DropDownService = dropDownFilterFactory;

        #region GET
        [HttpGet("GetUnassignedUsersDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetUnassignedUsersAsync()
        {
            try
            {
                return Ok(await DropDownService.GetUnassignedUsersAsync());
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

        [HttpGet("GetUOMDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetUOMAsync()
        {
            try
            {
                return Ok(await DropDownService.GetUOMAsync());
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

        [HttpGet("GetCustomersDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetCustomersAsync()
        {
            try
            {
                return Ok(await DropDownService.GetCustomersAsync());
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

        [HttpGet("GetProjectsDropDown/{CustomerID}")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetProjectsAsync(Guid CustomerID)
        {
            try
            {
                return Ok(await DropDownService.GetProjectsAsync(CustomerID));
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

        [HttpGet("GetStockGroupDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetStockGroupAsync()
        {
            try
            {
                return Ok(await DropDownService.GetStockGroupAsync());
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

        [HttpGet("GetOrderStatusListDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetOrderStatusListAsync()
        {
            try
            {
                return Ok(await DropDownService.GetOrderStatusListAsync());
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

        [HttpGet("GetInventoryStatusListDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetInventoryStatusListAsync()
        {
            try
            {
                return Ok(await DropDownService.GetInventoryStatusListAsync());
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
        [HttpPost("GetPOListGroupDropDown")]
        public async Task<ActionResult<List<DropDownFilterDTO>>> GetPOListGroupAsync([FromBody] POListFilterDTO pOListFilterDTO)
        {
            try
            {
                return Ok(await DropDownService.GetPOListGroupAsync(pOListFilterDTO));
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
