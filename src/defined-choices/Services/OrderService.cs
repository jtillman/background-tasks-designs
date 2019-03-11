using System;
using DefinedCaller.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using QueueT.Tasks;

namespace DefinedCaller.Services
{

    public class OrderService : IOrderService
    {
        public const string QueueName = "orderQueue";

        private readonly ILogger _logger;

        private readonly OrderDbContext _dbContext;

        private readonly ITaskService _taskService;

        public OrderService(
            ILogger<OrderService> logger,
            OrderDbContext dbContext,
            ITaskService taskService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _taskService = taskService;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId) => await _dbContext.Orders.SingleAsync(o => o.Id == orderId);

        public async Task CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            await _taskService.DelayAsync<OrderService>(service => service.ProcessOrderAsync(order.Id));
        }

        [QueuedTask(Name="ProcessOrder", Queue = QueueName)]
        public async Task ProcessOrderAsync(string orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            await _ProcessOrderAsync(order);
        }

        private async Task _ProcessOrderAsync(Order order)
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
