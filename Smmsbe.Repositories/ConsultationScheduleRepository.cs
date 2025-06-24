using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class ConsultationScheduleRepository :Repository<ConsultationSchedule>, IConsultationScheduleRepository
    {
        public ConsultationScheduleRepository(SMMSContext _context) : base(_context) { }

        public override async Task<ConsultationSchedule> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.ConsultationScheduleId == id);
        }
    }
}
