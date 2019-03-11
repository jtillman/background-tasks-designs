using System.Threading;
using System.Threading.Tasks;

namespace DefinedCaller
{
    public interface IOrderBackgroundService
    {
        Task BackgroundOrderProcessingAsync(string orderId);
        Task RunBackgroundWorkAsync(CancellationToken stoppingToken);
    }
}
