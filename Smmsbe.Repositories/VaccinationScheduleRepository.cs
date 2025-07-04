using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;
using Smmsbe.Repositories.Interfaces;

namespace Smmsbe.Repositories
{
    public class VaccinationScheduleRepository : Repository<VaccinationSchedule>, IVaccinationScheduleRepository
    {
        public VaccinationScheduleRepository(SMMSContext _context) : base(_context) { }

        public override async Task<VaccinationSchedule> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.VaccinationScheduleId == id);
        }

        public async Task<bool> VaccineScheduleIdExsistAsync(int id)
        {
            return await Table.AnyAsync(y => y.VaccinationScheduleId == id);
        }
    }
}
