using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;

namespace Smmsbe.Repositories.Interfaces
{
    public interface IHealthCheckupScheduleRepository : IRepository<HealthCheckSchedule>
    {
        Task<bool> HealthCheckupScheduleIdExsistAsync(int id);
    }
}
