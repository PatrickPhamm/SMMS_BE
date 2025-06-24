using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class MedicationRepository : Repository<Medication>, IMedicationRepository
    {
        public MedicationRepository(SMMSContext _context) : base(_context) { }

        public override async Task<Medication> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.MedicationId == id);
        }
    }
}
