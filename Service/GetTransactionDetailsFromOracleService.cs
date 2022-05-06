using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransApplicationService.LogService;

namespace TransApplicationService.Service
{
    class GetTransactionDetailsFromOracleService
    {

        internal class OracleTransactionDetailService : IHostedService, IDisposable
        {
            private Timer _timer;
            private readonly ILogger _logger;
            private static object _locker = new object();
            private readonly IConfiguration _config;
            private readonly IHostEnvironment _host;
            private readonly IServiceScopeFactory _scopeFactory;

            public OracleTransactionDetailService(ILogger<OracleTransactionDetailService> logger, IConfiguration config,
               IServiceScopeFactory scopeFactory, IHostEnvironment host)
            {
                _logger = logger;
                _config = config;
                _scopeFactory = scopeFactory;
                _host = host;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
                _logger.LogInformation("Timed Background Service is starting.");
                _timer = new Timer(DoWork, null, TimeSpan.Zero,
                  TimeSpan.FromSeconds(60));
                return Task.CompletedTask;
            }

            private void DoWork(object state)
            {
                var hasLock = false;
                try
                {
                    Monitor.TryEnter(_locker, ref hasLock);
                    if (!hasLock)
                    {
                        return;
                    }
                    //Write your function here//

                }
                catch (Exception ex)
                {

                    new LogWriter("OracleDBService-" + ex.Message + " | " + ex.InnerException + " | " + ex.StackTrace, _host, "GetTransactionOracleService");
                }
                finally
                {
                    if (hasLock)
                    {
                        Monitor.Exit(_locker);
                    }
                }
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                _logger.LogInformation("Timed Background Service is stopping.");

                _timer?.Change(Timeout.Infinite, 0);

                return Task.CompletedTask;
            }


            public void Dispose()
            {
                _timer?.Dispose();
            }
        }
    }
}
