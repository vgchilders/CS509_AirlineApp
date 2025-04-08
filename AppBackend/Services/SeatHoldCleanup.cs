
using AppBackend.Interfaces;

namespace AppBackend.Services
{
    public class SeatHoldCleanupSerivce : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger <SeatHoldCleanupSerivce> _logger ;

        public SeatHoldCleanupSerivce(IServiceProvider serviceProvider,ILogger <SeatHoldCleanupSerivce> logger){
            _serviceProvider = serviceProvider;
            _logger = logger;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {   
            while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var seatRepo = scope.ServiceProvider.GetRequiredService<ISeatRepository>();

            try
            {
                await seatRepo.ExprireStaleSeatHoldsAsync();
                _logger.LogInformation("[SeatHoldCleanup] Expired stale seat holds at {time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SeatHoldCleanup] Error while expiring stale seat holds");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // run every 5 minutes
        }

        }
    }

}