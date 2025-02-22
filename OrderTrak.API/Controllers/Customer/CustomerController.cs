using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO.Customer;
using OrderTrak.API.Services.Customer;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #region POST
        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerCreateDTO customerCreateDTO)
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
        #endregion
    }
}
