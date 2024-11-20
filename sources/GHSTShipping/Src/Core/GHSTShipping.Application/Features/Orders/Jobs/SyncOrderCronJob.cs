using GHSTShipping.Application.Features.Orders.Commands;
using GHSTShipping.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Jobs
{
    public class SyncOrderCronJob : CronJobService
    {
        private readonly IMediator _mediator;
        public SyncOrderCronJob(
            ILogger<SyncOrderCronJob> logger,
            IMediator mediator)
            : base(logger, TimeSpan.FromMinutes(60)) // Chạy mỗi 60 phút
        {
            _mediator = mediator;
        }

        public override Task DoWorkAsync(CancellationToken cancellationToken)
        {
            string message = $"SyncOrderCronJob is running at: {DateTime.Now}";
            Console.WriteLine(message);
            Serilog.Log.Information(message);

            _mediator.Send(new GHN_JobSyncOrderCommand(), cancellationToken);

            return Task.CompletedTask;
        }
    }
}
