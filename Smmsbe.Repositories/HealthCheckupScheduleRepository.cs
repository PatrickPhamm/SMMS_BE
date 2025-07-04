using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class HealthCheckupScheduleRepository : Repository<HealthCheckSchedule>, IHealthCheckupScheduleRepository
    {
        public HealthCheckupScheduleRepository(SMMSContext _context) : base(_context) { }

        public override async Task<HealthCheckSchedule> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.HealthCheckScheduleId == id);
        }

        public async Task<bool> HealthCheckupScheduleIdExsistAsync(int id)
        {
            return await Table.AnyAsync(y => y.HealthCheckScheduleId == id);
        }
    }
}
