using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;

namespace Smmsbe.Services.Interfaces
{
    public interface IVaccinationScheduleService
    {
        Task<VaccinationSchedule> GetById(int id);
        Task<List<GetVaccinationScheduleByFormResponse>> GetByForm(int formId);
        Task<VaccinationSchedule> AddVaccinationScheduleAsync(AddVaccinationScheduleRequest request);
        Task<List<VaccinationScheduleResponse>> SearchVaccinationScheduleAsync(SearchVaccinationScheduleRequest request);
        Task<VaccinationSchedule> UpdateVaccinationScheduleAsync(UpdateVaccinationScheduleRequest request);
        Task<bool> DeleteVaccinationScheduleAsync(int id);
    }
}
