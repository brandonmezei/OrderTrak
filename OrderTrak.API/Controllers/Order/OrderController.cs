using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Order;
using OrderTrak.API.Services.Order;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Order")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService orderService = orderService;

        #region GET
        [HttpGet("GetOrderHeader/{orderID}")]
        public async Task<ActionResult<OrderHeaderDTO>> GetOrderHeaderAsync(Guid orderID)
        {
            try
            {
                return Ok(await orderService.GetOrderHeaderAsync(orderID));
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
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Guid>> CreateOrderAsync([FromBody] OrderCreateDTO orderCreateDTO)
        {
            try
            {
                return Ok(await orderService.CreateOrderAsync(orderCreateDTO));
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

        [HttpPost("UpdateOrderHeader")]
        public async Task<ActionResult> UpdateOrderHeaderAsync([FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                await orderService.UpdateOrderHeaderAsync(orderHeaderUpdateDTO);
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

        [HttpPost("SearchOrder")]
        public async Task<ActionResult<PagedTable<OrderSearchReturnDTO>>> SearchOrderAsync([FromBody] SearchQueryDTO searchQuery)
        {
            try
            {
                return Ok(await orderService.SearchOrderAsync(searchQuery));
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
