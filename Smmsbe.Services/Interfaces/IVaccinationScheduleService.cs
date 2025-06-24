using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
