using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Enum;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.Services
{
    public class VaccinationResultService : IVaccinationResultService
    {
        private readonly IVaccinationResultRepository _vaccinationResultRepository;
        public VaccinationResultService(IVaccinationResultRepository vaccinationResultRepository)
        {
            _vaccinationResultRepository = vaccinationResultRepository;
        }

        #region getId v1
        /*public async Task<VaccinationResult> GetById(int id)
        {
            var entity = await _vaccinationResultRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }*/
        #endregion

        public async Task<VaccinationResultResponse> GetById(int id)
        {
            var entity = await _vaccinationResultRepository.GetAll()
                .Include(x => x.Nurse)
                .FirstOrDefaultAsync(x => x.VaccinationResultId == id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return new VaccinationResultResponse
            {
                VaccinationResultId = entity.VaccinationResultId,
                VaccinationScheduleId = entity.VaccinationScheduleId,
                HealthProfileId = entity.HealthProfileId,
                NurseId = entity.NurseId,
                NurseName = $"{entity.Nurse?.FullName}",
                Status = ((ResultStatus)entity.Status).ToString(),
                DoseNumber = entity.DoseNumber,
                Note = entity.Note
            };
        }

        public async Task<List<VaccinationResultResponse>> GetResultsBySchedule(int scheduleId)
        {
            return await _vaccinationResultRepository.GetAll()
                                        .Where(x => x.VaccinationScheduleId == scheduleId)
                                        .Select(x => new VaccinationResultResponse
                                        {
                                            VaccinationResultId = x.VaccinationResultId,
                                            VaccinationScheduleId = x.VaccinationScheduleId,
                                            HealthProfileId = x.HealthProfileId,
                                            NurseId = x.NurseId,
                                            NurseName = "",
                                            Status = ((ResultStatus)x.Status).ToString(),
                                            DoseNumber = x.DoseNumber,
                                            Note = x.Note
                                        }).ToListAsync();
        }

        public async Task<List<GetVaccinationResultByProfileResponse>> GetResultsByHealthProfile(int profileId)
        {
            return await _vaccinationResultRepository.GetAll()
                                       .Where(x => x.HealthProfileId == profileId)
                                       .Select(x => new GetVaccinationResultByProfileResponse
                                       {
                                           VaccinationResultId = x.VaccinationResultId,
                                           VaccinationScheduleId = x.VaccinationScheduleId,
                                           NurseId = x.NurseId,
                                           NurseName = "",
                                           Status = ((ResultStatus)x.Status).ToString(),
                                           DoseNumber = x.DoseNumber,
                                           Note = x.Note,
                                           HealthProfile = new HealthProfileResponse() 
                                           {
                                               HealthProfileId = x.HealthProfile.HealthProfileId,
                                               StudentId = x.HealthProfile.StudentId,
                                               Allergies = x.HealthProfile.Allergies,
                                               BloodType = x.HealthProfile.BloodType
                                           }
                                       }).ToListAsync();
        }

        #region AddVac v1
        /*public async Task<VaccinationResult> AddVaccinationResultAsync(AddVaccinationResultRequest request)
        {
            var newVac = new VaccinationResult
            {
                VaccinationScheduleId = request.VaccinationScheduleId,
                HealthProfileId = request.HealthProfileId,
                NurseId = request.NurseId,
                Status = request.Status,
                DoseNumber = request.DoseNumber,
                Note = request.Note
            };

            return await _vaccinationResultRepository.Insert(newVac);
        }*/
        #endregion

        public async Task<VaccinationResultResponse> AddVaccinationResultAsync(AddVaccinationResultRequest request)
        {
            var vac = new VaccinationResult
            {
                VaccinationScheduleId = request.VaccinationScheduleId,
                HealthProfileId = request.HealthProfileId,
                NurseId = request.NurseId,
                Status = 1,
                DoseNumber = request.DoseNumber,
                Note = request.Note
            };

            var addVac = await _vaccinationResultRepository.Insert(vac);

            var entityWithNu = await _vaccinationResultRepository.GetAll()
                .Include(x => x.Nurse)
                .FirstOrDefaultAsync(x => x.VaccinationResultId == addVac.VaccinationResultId);

            return new VaccinationResultResponse
            {
                VaccinationResultId = addVac.VaccinationResultId,
                VaccinationScheduleId = addVac.VaccinationScheduleId,
                HealthProfileId = addVac.HealthProfileId,
                NurseId = addVac.NurseId,
                NurseName = $"{addVac.Nurse?.FullName}",
                Status = ((ResultStatus)addVac.Status).ToString(),
                DoseNumber = addVac.DoseNumber,
                Note = addVac.Note
            };
        }

        public async Task<List<VaccinationResultResponse>> SearchVaccinationResultAsync(SearchVaccinationResultRequest request)
        {
            var query = _vaccinationResultRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.VaccinationScheduleId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.Status.ToString()) && s.Status.ToString().Contains(request.Keyword)));

            var searchVa = await query.Select(h => new VaccinationResultResponse
            {
                VaccinationResultId = h.VaccinationResultId,
                VaccinationScheduleId = h.VaccinationScheduleId,
                HealthProfileId = h.HealthProfileId,
                NurseId = h.NurseId,
                NurseName = $"{h.Nurse.FullName}",
                Status = ((ResultStatus)h.Status).ToString(),
                DoseNumber = h.DoseNumber,
                Note = h.Note
            }).ToListAsync();

            return searchVa;
        }

        public async Task<VaccinationResult> UpdateVaccinationResultAsync(UpdateVaccinationResultRequest request)
        {
            var updateVaccinationSchedule = await _vaccinationResultRepository.GetById(request.VaccinationResultId);
            if (updateVaccinationSchedule == null) throw AppExceptions.NotFoundId();

            updateVaccinationSchedule.NurseId = request.NurseId;
            updateVaccinationSchedule.DoseNumber = request.DoseNumber;
            updateVaccinationSchedule.Note = request.Note;

            await _vaccinationResultRepository.Update(updateVaccinationSchedule);
            return updateVaccinationSchedule;
        }

        public async Task<bool> DeleteVaccinationResultAsync(int id)
        {
            try
            {
                var deleted = await _vaccinationResultRepository.GetById(id);
                if (deleted == null) throw AppExceptions.NotFoundId();

                await _vaccinationResultRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CompleteVaccinationResultAsync(int id)
        {
            var consultation = await _vaccinationResultRepository.GetById(id);
            if (consultation == null) return false;

            consultation.Status = (int)ResultStatus.Completed;

            await _vaccinationResultRepository.Update(consultation);

            return true;
        }
    }
}
