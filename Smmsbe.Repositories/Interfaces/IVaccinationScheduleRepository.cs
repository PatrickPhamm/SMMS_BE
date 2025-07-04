using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Infrastructure;

namespace Smmsbe.Repositories.Interfaces
{
    public interface IVaccinationScheduleRepository : IRepository<VaccinationSchedule>
    {
        Task<bool> VaccineScheduleIdExsistAsync(int id);
    }
}
