using System.Collections.Specialized;
using Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox;
using Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;
using Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using Modules.UserAccess.Infrastructure.Configuration.Quartz.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Serilog;

namespace Modules.UserAccess.Infrastructure.Configuration.Quartz
{
    internal static class QuartzStartup
    {
        internal static void Initialize(ILogger logger)
        {
            logger.Information("Quartz starting...");

            var schedulerConfiguration = new NameValueCollection
            {
                {"quartz.scheduler.instanceName", "CompanyOfOneFinances"}
            };

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            LogProvider.SetCurrentLogProvider(new SerilogLogProvider(logger));

            scheduler.Start().GetAwaiter().GetResult();

            var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            var trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();

            scheduler
                .ScheduleJob(processOutboxJob, trigger)
                .GetAwaiter().GetResult();

            var processInboxJob = JobBuilder.Create<ProcessInboxJob>().Build();
            var processInboxTrigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();

            scheduler
                .ScheduleJob(processInboxJob, processInboxTrigger)
                .GetAwaiter().GetResult();

            var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
            var triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();
            scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();

            ScheduleExpireUserRegistrationsJob(scheduler);

            logger.Information("Quartz started.");
        }

        private static void ScheduleExpireUserRegistrationsJob(IScheduler scheduler)
        {
            var expireUserRegistrationsJob = JobBuilder.Create<ExpireUserRegistrationsJob>().Build();
            var triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("50 * * ? * *")
                    .Build();
            
            scheduler.ScheduleJob(expireUserRegistrationsJob, triggerCommandsProcessing).GetAwaiter().GetResult();
        }
    }
}