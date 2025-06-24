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
    public class HealthCheckResultService : IHealthCheckResultService
    {
        private readonly IHealthCheckResultRepository _healthCheckResultRepository;
        public HealthCheckResultService(IHealthCheckResultRepository healthCheckResultRepository)
        {
            _healthCheckResultRepository = healthCheckResultRepository;
        }

        #region getId v1
        /*public async Task<HealthCheckResult> GetById(int id)
        {
            var entity = await _healthCheckResultRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }*/
        #endregion

        public async Task<HealthCheckResultResponse> GetById(int id)
        {
            var entity = await _healthCheckResultRepository.GetAll()
                .Include(x => x.Nurse)
                .FirstOrDefaultAsync(x => x.HealthCheckupRecordId == id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return new HealthCheckResultResponse
            {
                HealthCheckScheduleId = entity.HealthCheckScheduleId,
                NurseId = entity.NurseId,
                NurseName = $"{entity.Nurse.FullName}",
                HealthProfileId = entity.HealthProfileId,
                Status = ((ResultStatus)entity.Status).ToString(),
                Height = entity.Height,
                Weight = entity.Weight,
                LeftVision = entity.LeftVision,
                RightVision = entity.RightVision,
                Result = entity.Result,
                Note = entity.Note
            };
        }

        public async Task<List<HealthCheckResultResponse>> GetResultsBySchedule(int scheduleId)
        {
            return await _healthCheckResultRepository.GetAll()
                                        .Where(x => x.HealthCheckScheduleId == scheduleId)
                                        .Select(x => new HealthCheckResultResponse
                                        {
                                            HealthCheckupRecordId = x.HealthCheckupRecordId,
                                            HealthCheckScheduleId = x.HealthCheckScheduleId,
                                            HealthProfileId = x.HealthProfileId,
                                            NurseId = x.NurseId,
                                            NurseName = "",
                                            Status = ((ResultStatus)x.Status).ToString(),
                                            Height = x.Height,
                                            Weight = x.Weight,
                                            LeftVision = x.LeftVision,
                                            RightVision = x.RightVision,
                                            Result = x.Result,
                                            Note = x.Note
                                        }).ToListAsync();
        }

        public async Task<List<GetHealthCheckResultByProfileResponse>> GetResultsByHealthProfile(int profileId)
        {
            return await _healthCheckResultRepository.GetAll()
                                        .Where(x => x.HealthProfileId == profileId)
                                        .Select(x => new GetHealthCheckResultByProfileResponse
                                        {
                                            HealthCheckupRecordId = x.HealthCheckupRecordId,
                                            HealthCheckScheduleId = x.HealthCheckScheduleId,
                                            NurseId = x.NurseId,
                                            NurseName = "",
                                            Status = ((ResultStatus)x.Status).ToString(),
                                            Height = x.Height,
                                            Weight = x.Weight,
                                            LeftVision = x.LeftVision,
                                            RightVision = x.RightVision,
                                            Result = x.Result,
                                            Note = x.Note,
                                            HealthProfile = new HealthProfileResponse 
                                            { 
                                                HealthProfileId = x.HealthProfile.HealthProfileId,
                                                StudentId = x.HealthProfile.StudentId,
                                                Allergies = x.HealthProfile.Allergies,
                                                BloodType = x.HealthProfile.BloodType
                                            }
                                        }).ToListAsync();
        }

        public async Task<HealthCheckResultResponse> AddHealthCheckResultAsync(AddHealthCheckResultRequest request)
        {
            var newHea = new HealthCheckResult
            {
                HealthCheckScheduleId = request.HealthCheckScheduleId,
                NurseId = request.NurseId,
                HealthProfileId = request.HealthProfileId,
                Status = 1,
                Height = request.Height,
                Weight = request.Weight,
                LeftVision = request.LeftVision,
                RightVision = request.RightVision,
                Result = request.Result,
                Note = request.Note
            };

            var addHea = await _healthCheckResultRepository.Insert(newHea);

            var entityWithNurse = await _healthCheckResultRepository.GetAll()
                .Include(x => x.Nurse)
                .FirstOrDefaultAsync(x => x.HealthCheckupRecordId == addHea.HealthCheckupRecordId);

            return new HealthCheckResultResponse
            {
                HealthCheckupRecordId = addHea.HealthCheckupRecordId,
                HealthCheckScheduleId = addHea.HealthCheckScheduleId,
                NurseId = addHea.NurseId,
                NurseName = $"{addHea.Nurse?.FullName}",
                HealthProfileId = addHea.HealthProfileId,
                Status = ((ResultStatus)addHea.Status).ToString(),
                Height = addHea.Height,
                Weight = addHea.Weight,
                LeftVision = addHea.LeftVision,
                RightVision = addHea.RightVision,
                Result = addHea.Result,
                Note = addHea.Note
            };
        }

        public async Task<List<HealthCheckResultResponse>> SearchHealthCheckResultAsync(SearchHealthCheckResultRequest request)
        {
            var query = _healthCheckResultRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.HealthCheckupRecordId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.Status.ToString()) && s.Status.ToString().Contains(request.Keyword)));

            var searchHea = await query.Select(h => new HealthCheckResultResponse
            {
                HealthCheckupRecordId = h.HealthCheckupRecordId,
                HealthCheckScheduleId = h.HealthCheckScheduleId,
                NurseId = h.NurseId,
                NurseName = $"{h.Nurse.FullName}",
                HealthProfileId = h.HealthProfileId,
                Status = ((ResultStatus)h.Status).ToString(),
                Height = h.Height,
                Weight = h.Weight,
                LeftVision = h.LeftVision,
                RightVision = h.RightVision,
                Result = h.Result,
                Note = h.Note
            }).ToListAsync();

            return searchHea;
        }

        public async Task<HealthCheckResult> UpdateHealthCheckResultAsync(UpdateHealthCheckResultRequest request)
        {
            var updateVaccinationSchedule = await _healthCheckResultRepository.GetById(request.HealthCheckupRecordId);
            if (updateVaccinationSchedule == null) throw AppExceptions.NotFoundId();

            updateVaccinationSchedule.NurseId = request.NurseId;
            updateVaccinationSchedule.Height = request.Height;
            updateVaccinationSchedule.Weight = request.Weight;
            updateVaccinationSchedule.LeftVision = request.LeftVision;
            updateVaccinationSchedule.RightVision = request.RightVision;
            updateVaccinationSchedule.Result = request.Result;
            updateVaccinationSchedule.Note = request.Note;

            await _healthCheckResultRepository.Update(updateVaccinationSchedule);
            return updateVaccinationSchedule;
        }

        public async Task<bool> DeleteHealthCheckResultAsync(int id)
        {
            try
            {
                var deleted = await _healthCheckResultRepository.GetById(id);
                if (deleted == null) throw AppExceptions.NotFoundId();

                await _healthCheckResultRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CompleteCheckResultAsync(int id)
        {
            var checkRe = await _healthCheckResultRepository.GetById(id);
            if (checkRe == null) return false;

            checkRe.Status = (int)ResultStatus.Completed;

            await _healthCheckResultRepository.Update(checkRe);

            return true;
        }
    }
}
