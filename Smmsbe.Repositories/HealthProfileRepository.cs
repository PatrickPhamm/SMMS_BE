using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class HealthProfileRepository : Repository<HealthProfile>, IHealthProfileRepository
    {
        public HealthProfileRepository(SMMSContext _context) : base(_context) { }

        public override async Task<HealthProfile> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.HealthProfileId == id);
        }
    }
}
