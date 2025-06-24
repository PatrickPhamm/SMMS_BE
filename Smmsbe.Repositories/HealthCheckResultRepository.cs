using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Repositories
{
    public class HealthCheckResultRepository : Repository<HealthCheckResult>, IHealthCheckResultRepository
    {
        public HealthCheckResultRepository(SMMSContext _context) : base(_context) { }

        public override async Task<HealthCheckResult> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.HealthCheckupRecordId == id);
        }
    }
}
