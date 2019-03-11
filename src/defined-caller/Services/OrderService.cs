using System;
using DefinedCaller.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DefinedCaller;

namespace DefinedCaller.Services
{

    public class OrderService : IOrderService
    {
        private readonly ILogger _logger;

        private readonly OrderDbContext _dbContext;

        private readonly IOrderBackgroundService _backgroundService;

        public OrderService(
            ILogger<OrderService> logger,
            OrderDbContext dbContext,
            IOrderBackgroundService backgroundService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _backgroundService = backgroundService;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId) => await _dbContext.Orders.SingleAsync(o => o.Id == orderId);

        public async Task CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            await _backgroundService.BackgroundOrderProcessingAsync(order.Id);
        }

        public async Task ProcessOrderAsync(Order order)
        {
            _logger.LogInformation($"Starting order processing OrderId={order.Id}");
            await _ProcessPaymentAsync(order);
            await Task.Delay(TimeSpan.FromSeconds(1));
            _logger.LogInformation($"Ending order processing OrderId={order.Id}");
        }

        private async Task _ProcessPaymentAsync(Order order)
        {
            _logger.LogInformation($"Starting payment process for OrderId={order.Id}");
            await Task.Delay(TimeSpan.FromSeconds(5));
            _logger.LogInformation($"Ending payment process for OrderId={order.Id}");
        }
    }
}
