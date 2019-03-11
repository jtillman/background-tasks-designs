using DefinedCaller.Models;
using System.Threading.Tasks;

namespace DefinedCaller.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(string orderId);
    }
}
