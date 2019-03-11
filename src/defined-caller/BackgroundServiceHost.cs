using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DefinedCaller
{
    public class BackgroundServiceHost : BackgroundService
    {
        private readonly IOrderBackgroundService _backgroundService;

        public BackgroundServiceHost(
            IOrderBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => _backgroundService.RunBackgroundWorkAsync(stoppingToken);
    }
}
