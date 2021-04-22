using System.Threading.Tasks;
using Modules.UserAccess.Application.UserRegistrations.ExpireUserRegistrations;
using Modules.UserAccess.Infrastructure.Configuration.Processing;
using Quartz;

namespace Modules.UserAccess.Infrastructure.Configuration.Quartz.Jobs
{
    public class ExpireUserRegistrationsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.Execute(new ExpireUserRegistrationsCommand());
        }
    }
}