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

        [HttpGet("GetOrderShipping/{orderID}")]
        public async Task<ActionResult<OrderShipDTO>> GetOrderShippingAsync(Guid orderID)
        {
            try
            {
                return Ok(await orderService.GetOrderShippingAsync(orderID));
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

        [HttpGet("GetOrderActivation/{orderID}")]
        public async Task<ActionResult<OrderActivationDTO>> GetOrderActivationAsync(Guid orderID)
        {
            try
            {
                return Ok(await orderService.GetOrderActivationAsync(orderID));
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
        public async Task<ActionResult<PagedTable<OrderSearchReturnDTO>>> SearchOrderAsync([FromBody] OrderSearchDTO searchQuery)
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

        [HttpPost("CreateOrderLine")]
        public async Task<ActionResult> CreateOrderLineAsync([FromBody] OrderCreateLineDTO orderCreateLineDTO)
        {
            try
            {
                await orderService.CreateOrderLineAsync(orderCreateLineDTO);
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

        [HttpPost("UpdateOrderLine")]
        public async Task<ActionResult> UpdateOrderLineAsync([FromBody] OrderPartListUpdate orderPartListUpdateDTO)
        {
            try
            {
                await orderService.UpdateOrderLineAsync(orderPartListUpdateDTO);
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

        [HttpPost("UpdateOrderShipping")]
        public async Task<ActionResult> UpdateOrderShippingAsync([FromBody] OrderShipUpdateDTO orderShipUpdateDTO)
        {
            try
            {
                await orderService.UpdateOrderShippingAsync(orderShipUpdateDTO);
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

        [HttpPost("UpdateOrderActivation")]
        public async Task<ActionResult> UpdateOrderActivationAsync([FromBody] OrderActivationUpdateDTO orderActivationUpdateDTO)
        {
            try
            {
                await orderService.UpdateOrderActivationAsync(orderActivationUpdateDTO);
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

        [HttpPost("GetOrderLine")]
        public async Task<ActionResult<List<OrderPartListDTO>>> GetOrderLineAsync([FromBody] OrderPartListSearchDTO orderPartListSearchDTO)
        {
            try
            {
                return Ok(await orderService.GetOrderLineAsync(orderPartListSearchDTO));
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

        [HttpPost("CancelOrder")]
        public async Task<ActionResult> CancelOrderAsync([FromBody] OrderCancelDTO orderCancelDTO)
        {
            try
            {
                await orderService.CancelOrderAsync(orderCancelDTO);
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
        [HttpDelete("DeleteOrderLine/{lineID}")]
        public async Task<ActionResult> DeleteOrderLineAsync(Guid lineID)
        {
            try
            {
                await orderService.DeleteOrderLineAsync(lineID);
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
