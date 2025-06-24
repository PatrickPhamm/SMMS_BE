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
    public class HealthCheckScheduleService : IHealthCheckScheduleService
    {
        private readonly IHealthCheckupScheduleRepository _healthCheckupScheduleRepository;
        private readonly IFormRepository _formRepository;

        public HealthCheckScheduleService(IHealthCheckupScheduleRepository healthCheckupScheduleRepository
            , IFormRepository formRepository) 
        {
            _healthCheckupScheduleRepository = healthCheckupScheduleRepository;
            _formRepository = formRepository;
        }

        public async Task<HealthCheckSchedule> GetById(int id)
        {
            var entity = await _healthCheckupScheduleRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<List<GetHealthCheckScheduleByFormResponse>> GetByForm(int formId)
        {
            var getByFrom = await _healthCheckupScheduleRepository.GetAll()
                .Where(s => s.FormId == formId)
                .Select(s => new GetHealthCheckScheduleByFormResponse
                {
                    HealthCheckScheduleId = s.HealthCheckScheduleId,
                    ManagerId = s.ManagerId,
                    Name = s.Name,
                    CheckDate = s.CheckDate,
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

            return getByFrom;
        }

        public async Task<HealthCheckSchedule> AddHealthCheckScheduleAsync(AddHealthCheckScheduleRequest request)
        {
            var added = new HealthCheckSchedule
            {
                FormId = request.FormId,
                ManagerId = request.ManagerId,
                Name = request.Name,
                CheckDate = request.CheckDate,
                Location = request.Location,
                Note = request.Note
            };

            return await _healthCheckupScheduleRepository.Insert(added);
        }
        
        public async Task<List<HealthCheckScheduleResponse>> SearchHealthCheckScheduleAsync(SearchHealthCheckScheduleRequest request)
        {
            var query = _healthCheckupScheduleRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.HealthCheckScheduleId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.Name.ToString()) && s.Name.ToString().Contains(request.Keyword)));

            var searchHea = await query.Select(h => new HealthCheckScheduleResponse
            {
                HealthCheckScheduleId = h.HealthCheckScheduleId,
                FormId = h.FormId,
                ManagerId = h.ManagerId,
                Name = h.Name,
                CheckDate = h.CheckDate,
                Location = h.Location,
                Note = h.Note
            }).ToListAsync();

            return searchHea;
        }

        public async Task<HealthCheckSchedule> UpdateHealthCheckScheduleAsync(UpdateHealthCheckScheduleRequest request)
        {
            var updateHealthCheckSchedule = await _healthCheckupScheduleRepository.GetById(request.HealthCheckScheduleId);
            if (updateHealthCheckSchedule == null) throw AppExceptions.NotFoundId();

            updateHealthCheckSchedule.Name = request.Name;
            updateHealthCheckSchedule.CheckDate = request.CheckDate;
            updateHealthCheckSchedule.Location = request.Location;
            updateHealthCheckSchedule.Note = request.Note;

            await _healthCheckupScheduleRepository.Update(updateHealthCheckSchedule);
            return updateHealthCheckSchedule;
        }

        public async Task<bool> DeleteHealthCheckScheduleAsync(int id)
        {
            try
            {
                var deleted = await _healthCheckupScheduleRepository.GetById(id);
                if (deleted == null) throw AppExceptions.NotFoundId();

                await _healthCheckupScheduleRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
