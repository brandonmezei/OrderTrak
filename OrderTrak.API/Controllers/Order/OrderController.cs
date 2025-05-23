﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "PickingOrderShipping")]
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
        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Picking")]
        [HttpPost("PickToOrder")]
        public async Task<ActionResult> PickToOrderAsync([FromBody] OrderPickDTO orderPickDTO)
        {
            try
            {
                await orderService.PickToOrderAsync(orderPickDTO);
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

        [Authorize(Policy = "Picking")]
        [HttpPost("IsDonePicking")]
        public async Task<ActionResult<bool>> IsDonePickingAsync([FromBody] OrderPickDoneDTO orderPickDoneDTO)
        {
            try
            {
                return Ok(await orderService.IsDonePickAsync(orderPickDoneDTO));
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

        [Authorize(Policy = "Order")]
        [HttpPost("RemovePickFromOrder")]
        public async Task<ActionResult> RemovePickFromOrderAsync([FromBody] OrderPickRemoveDTO orderPickRemoveDTO)
        {
            try
            {
                await orderService.RemovePickFromOrderAsync(orderPickRemoveDTO);
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

        [Authorize(Policy = "Shipping")]
        [HttpPost("CreateTrackingForOrder")]
        public async Task<ActionResult> CreateTrackingForOrderAsync([FromBody] OrderCreateTrackingDTO orderCreateTrackingDTO)
        {
            try
            {
                await orderService.CreateTrackingForOrderAsync(orderCreateTrackingDTO);
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

        [HttpPost("SearchOrderTracking")]
        public async Task<ActionResult<PagedTable<OrderTrackingSearchReturnDTO>>> SearchOrderTrackingAsync([FromBody] OrderTrackingSearchDTO searchQuery)
        {
            try
            {
                return Ok(await orderService.SearchOrderTrackingAsync(searchQuery));
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

        [Authorize(Policy = "Shipping")]
        [HttpPost("CompleteOrderShipping")]
        public async Task<ActionResult> CompleteOrderShippingAsync([FromBody] OrderCompleteShippingDTO orderCompleteShippingDTO)
        {
            try
            {
                await orderService.CompleteShippingOrderAsync(orderCompleteShippingDTO);
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
        [Authorize(Policy = "Order")]
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

        [Authorize(Policy = "Shipping")]
        [HttpDelete("DeleteTrackingFromOrder/{trackingID}")]
        public async Task<ActionResult> DeleteTrackingFromOrderAsync(Guid trackingID)
        {
            try
            {
                await orderService.DeleteTrackingFromOrderAsync(trackingID);
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
