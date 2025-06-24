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
    public class ConsultationScheduleService : IConsultationScheduleService
    {
        private readonly IConsultationScheduleRepository _consultationScheduleRepository;
        private readonly IConsultationFormRepository _consultationFormRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;

        public ConsultationScheduleService(IConsultationScheduleRepository consultationScheduleRepository
            , IStudentRepository studentRepository)
        {
            _consultationScheduleRepository = consultationScheduleRepository;
            _studentRepository = studentRepository;
        }

        public async Task<ConsultationSchedule> GetById(int id)
        {
            var entity = await _consultationScheduleRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<ConsultationScheduleResponse> GetByIdAsync(int id)
        {
            var entity = await _consultationScheduleRepository.GetAll()
                                                                    .Where(x => x.ConsultationScheduleId == id)
                                                                    .Select(x => new ConsultationScheduleResponse
                                                                    {
                                                                        ConsultationScheduleId = x.ConsultationScheduleId,
                                                                        NurseId = x.NurseId,
                                                                        StudentId = x.StudentId,
                                                                        ConsultDate = x.ConsultDate,
                                                                        Location = x.Location,
                                                                    }).FirstOrDefaultAsync();

            if (entity == null) throw AppExceptions.NotFoundId();
            return entity;
        }

        public async Task<List<ConsultationScheduleByStudentResponse>> GetByStudent(int studentId)
        {
            return await _consultationScheduleRepository.GetAll()
                .Where(x => x.StudentId == studentId)
                .Select(x => new ConsultationScheduleByStudentResponse
                {
                    ConsultationScheduleId = x.ConsultationScheduleId,
                    NurseId = x.NurseId,
                    Location = x.Location,
                    ConsultDate = x.ConsultDate,
                    Student = new StudentResponse
                    {
                        StudentId = x.Student.StudentId,
                        FullName = x.Student.FullName,
                        ClassName = x.Student.ClassName,
                        DateOfBirth = x.Student.DateOfBirth,    
                        Gender = x.Student.Gender,
                        StudentNumber = x.Student.StudentNumber,
                        Parent = null
                    }
                }).ToListAsync();
        }

        public async Task<ConsultationSchedule> AddConsultationScheduleAsync(AddConsultationScheduleRequest request)
        {
            var newCon = new ConsultationSchedule
            {
                NurseId = request.NurseId,
                StudentId = request.StudentId,
                Location = request.Location,
                ConsultDate = request.ConsultDate
            };

            return await _consultationScheduleRepository.Insert(newCon);
        }

        public async Task<List<ConsultationScheduleResponse>> SearchConsultationScheduleAsync(SearchConsultationScheduleRequest request)
        {
            var query = _consultationScheduleRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.ConsultationScheduleId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.Location.ToString()) && s.Location.ToString().Contains(request.Keyword)));

            var searchCo = await query.Select(x => new ConsultationScheduleResponse
            {
                ConsultationScheduleId = x.ConsultationScheduleId,
                NurseId = x.NurseId,
                StudentId = x.StudentId,
                Location = x.Location,
                ConsultDate = x.ConsultDate
            }).ToListAsync();

            return searchCo;
        }

        public async Task<ConsultationSchedule> UpdateConsultationScheduleAsync(UpdateConsultationScheduleRequest request)
        {
            var updateConsentForm = await _consultationScheduleRepository.GetById(request.ConsultationScheduleId);
            if (updateConsentForm == null) throw AppExceptions.NotFoundId();

            updateConsentForm.ConsultationScheduleId = request.ConsultationScheduleId;
            updateConsentForm.NurseId = request.NurseId;
            updateConsentForm.StudentId = request.StudentId;
            updateConsentForm.Location = request.Location;
            updateConsentForm.ConsultDate = request.ConsultDate;

            await _consultationScheduleRepository.Update(updateConsentForm);
            return updateConsentForm;
        }

        public async Task<bool> DeleteConsultationScheduleAsync(int id)
        {
            try
            {
                var deleted = await _consultationScheduleRepository.GetById(id);
                if (deleted == null) throw AppExceptions.NotFoundId();

                await _consultationScheduleRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
