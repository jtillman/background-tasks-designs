using DefinedCaller.Models;
using DefinedCaller.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DefinedCaller.Controllers
{
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        private object _OrderToJsonObject(Order order) => new { id = order.Id };

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderAsync(string orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (null == order)
                return NotFound();
            return Ok(_OrderToJsonObject(order));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateOrderAsync()
        {
            var order = new Order { Id = Guid.NewGuid().ToString() };
            await _orderService.CreateOrderAsync(order);
            return Created($"api/orders/{order.Id}", _OrderToJsonObject(order));
        }
    }
}
