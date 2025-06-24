using Smmsbe.Repositories.Entities;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IVaccinationResultService
    {
        //Task<VaccinationResult> GetById(int id);
        Task<VaccinationResultResponse> GetById(int id);
        Task<List<VaccinationResultResponse>> GetResultsBySchedule(int scheduleId);
        Task<List<GetVaccinationResultByProfileResponse>> GetResultsByHealthProfile(int profileId);
        //Task<VaccinationResult> AddVaccinationResultAsync(AddVaccinationResultRequest request);
        Task<VaccinationResultResponse> AddVaccinationResultAsync(AddVaccinationResultRequest request);
        Task<List<VaccinationResultResponse>> SearchVaccinationResultAsync(SearchVaccinationResultRequest request);
        Task<VaccinationResult> UpdateVaccinationResultAsync(UpdateVaccinationResultRequest request);
        Task<bool> DeleteVaccinationResultAsync(int id);
        Task<bool> CompleteVaccinationResultAsync(int id);
    }
}
