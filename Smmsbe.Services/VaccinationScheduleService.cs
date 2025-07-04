using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Enum;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.Services
{
    public class VaccinationScheduleService : IVaccinationScheduleService
    {
        private readonly IVaccinationScheduleRepository _vaccinationScheduleRepository;
        private readonly IHealthCheckupScheduleRepository _healthCheckupScheduleRepository;
        private readonly IFormRepository _formRepository;

        public VaccinationScheduleService(IVaccinationScheduleRepository vaccinationScheduleRepository,
            IHealthCheckupScheduleRepository healthCheckupScheduleRepository,
            IFormRepository formRepository)
        {
            _vaccinationScheduleRepository = vaccinationScheduleRepository;
            _healthCheckupScheduleRepository = healthCheckupScheduleRepository;
            _formRepository = formRepository;
        }

        public async Task<VaccinationSchedule> GetById(int id)
        {
            var entity = await _vaccinationScheduleRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<List<GetVaccinationScheduleByFormResponse>> GetByForm(int formId)
        {
            var getByForm = await _vaccinationScheduleRepository.GetAll()
                .Where(s => s.FormId == formId)
                .Select(s => new GetVaccinationScheduleByFormResponse
                {
                    VaccinationScheduleId = s.VaccinationScheduleId,
                    ManagerId = s.ManagerId,
                    Name = s.Name,
                    ScheduleDate = s.ScheduleDate,
                    Location = s.Location,
                    Note = s.Note,
                    Form = new FormResponse
                    {
                        FormId = s.Form.FormId,
                        Title = s.Form.Title,
                        ClassName = s.Form.ClassName,
                        SentDate = s.Form.SentDate,
                        Content = s.Form.Content,
                        Type = ((FormType)s.Form.Type).ToString()
                    }
                }).ToListAsync();

            return getByForm;
        }

        public async Task<VaccinationSchedule> AddVaccinationScheduleAsync(AddVaccinationScheduleRequest request)
        {
            // Kiểm tra xem đã có lịch tiêm chủng nào trong ngày này chưa
            var existingVaccination = await _vaccinationScheduleRepository.GetAll()
                .Where(x => x.ScheduleDate.HasValue && request.ScheduleDate.HasValue &&
                           x.ScheduleDate.Value.Date == request.ScheduleDate.Value.Date)
                .FirstOrDefaultAsync();

            if (existingVaccination != null)
            {
                throw AppExceptions.ScheduleAlreadyExist();
            }

            // Kiểm tra xem đã có lịch khám sức khỏe nào trong ngày này chưa
            var existingHealthCheck = await _healthCheckupScheduleRepository.GetAll()
                .Where(x => x.CheckDate.HasValue && request.ScheduleDate.HasValue &&
                           x.CheckDate.Value.Date == request.ScheduleDate.Value.Date)
                .FirstOrDefaultAsync();

            if (existingHealthCheck != null)
            {
                throw AppExceptions.ScheduleAlreadyExist();
            }

            var added = new VaccinationSchedule
            {
                FormId = request.FormId,
                ManagerId = request.ManagerId,
                Name = request.Name,
                ScheduleDate = request.ScheduleDate,
                Location = request.Location,
                Note = request.Note
            };

            return await _vaccinationScheduleRepository.Insert(added);
        }

        public async Task<List<VaccinationScheduleResponse>> SearchVaccinationScheduleAsync(SearchVaccinationScheduleRequest request)
        {
            var query = _vaccinationScheduleRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.VaccinationScheduleId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.Name.ToString()) && s.Name.ToString().Contains(request.Keyword)));

            var searchVa = await query.Select(h => new VaccinationScheduleResponse
            {
                VaccinationScheduleId = h.VaccinationScheduleId,
                FormId = h.FormId,
                ManagerId = h.ManagerId,
                Name = h.Name,
                ScheduleDate = h.ScheduleDate,
                Location = h.Location,
                Note = h.Note
            }).ToListAsync();

            return searchVa;
        }

        public async Task<VaccinationSchedule> UpdateVaccinationScheduleAsync(UpdateVaccinationScheduleRequest request)
        {
            var updateVaccinationSchedule = await _vaccinationScheduleRepository.GetById(request.VaccinationScheduleId);
            if (updateVaccinationSchedule == null) throw AppExceptions.NotFoundId();

            updateVaccinationSchedule.Name = request.Name;
            updateVaccinationSchedule.ScheduleDate = request.ScheduleDate;
            updateVaccinationSchedule.Location = request.Location;
            updateVaccinationSchedule.Note = request.Note;

            await _vaccinationScheduleRepository.Update(updateVaccinationSchedule);
            return updateVaccinationSchedule;
        }

        public async Task<bool> DeleteVaccinationScheduleAsync(int id)
        {
            try
            {
                var deleted = await _vaccinationScheduleRepository.GetById(id);
                if (deleted == null) throw AppExceptions.NotFoundId();

                await _vaccinationScheduleRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
