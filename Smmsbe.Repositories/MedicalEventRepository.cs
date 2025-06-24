using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class MedicalEventRepository : Repository<MedicalEvent>, IMedicalEventRepository
    {
        public MedicalEventRepository(SMMSContext _context) : base(_context) { }

        public override async Task<MedicalEvent> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.EventId == id);
        }
    }
}
