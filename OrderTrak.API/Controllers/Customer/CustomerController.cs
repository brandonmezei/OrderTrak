using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Customer;
using OrderTrak.API.Services.Customer;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Customer")]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        #region GET
        [HttpGet("GetCustomer/{customerId}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerAsync(Guid customerId)
        {
            try
            {
                return Ok(await _customerService.GetCustomerAsync(customerId));
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
        [HttpPost("CreateCustomer")]
        public async Task<ActionResult<Guid>> CreateCustomerAsync([FromBody] CustomerCreateDTO customerCreateDTO)
        {
            try
            {
                return Ok(await _customerService.CreateCustomerAsync(customerCreateDTO));
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

        [HttpPost("UpdateCustomer")]
        public async Task<ActionResult> UpdateCustomerAsync([FromBody] CustomerUpdateDTO customerUpdateDTO)
        {
            try
            {
                await _customerService.UpdateCustomerAsync(customerUpdateDTO);
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

        [HttpPost("SearchCustomer")]
        public async Task<ActionResult<PagedTable<CustomerSearchReturnDTO>>> SearchCustomersAsync([FromBody] CustomerSearchDTO customerSearchDTO)
        {
            try
            {
                return Ok(await _customerService.SearchCustomersAsync(customerSearchDTO));
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
        [HttpDelete("DeleteCustomer/{customerId}")]
        public async Task<ActionResult> DeleteCustomerAsync(Guid customerId)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(customerId);
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
