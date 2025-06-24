using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services
{
    public class MedicalEventService : IMedicalEventService
    {
        private readonly IMedicalEventRepository _medicalEventRepository;
        public MedicalEventService(IMedicalEventRepository medicalEventRepository)
        {
            _medicalEventRepository = medicalEventRepository;
        }

        public async Task<MedicalEvent> GetById(int id)
        {
            var entity = await _medicalEventRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<List<MedicalEventResponse>> GetMedicalByStudent(int studentId)
        {
            var entity = await _medicalEventRepository.GetAll()
                                            .Include(x => x.Student)
                                            .Where(x => x.StudentId == studentId)
                                            .Select(x => new MedicalEventResponse
                                            {
                                                EventId = x.EventId,
                                                StudentId = x.StudentId,
                                                NurseId = x.NurseId,
                                                EventName = x.EventName,
                                                EventDate = x.EventDate,
                                                Symptoms = x.Symptoms,
                                                ActionTaken = x.ActionTaken,
                                                Note = x.Note
                                            }).ToListAsync();

            if (entity == null) throw AppExceptions.NotFoundId();
            return entity;
        }

        public async Task<MedicalEvent> AddMedicalEventAsync(AddMedicalEventRequest request)
        {
            var added = new MedicalEvent
            {
                StudentId = request.StudentId,
                NurseId = request.NurseId,
                EventName = request.EventName,
                EventDate = request.EventDate,
                Symptoms = request.Symptoms,
                ActionTaken = request.ActionTaken,
                Note = request.Note
            };

            return await _medicalEventRepository.Insert(added);
        }

        public async Task<List<MedicalEventResponse>> SearchMedicalEventAsync(SearchMedicalEventRequest request)
        {
            var query = _medicalEventRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.EventId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.EventName.ToString()) && s.EventName.ToString().Contains(request.Keyword)));

            var searchVa = await query.Select(h => new MedicalEventResponse
            {
                EventId = h.EventId,
                StudentId = h.StudentId,
                NurseId = h.NurseId,
                EventName = h.EventName,
                EventDate = h.EventDate,
                Symptoms = h.Symptoms,
                ActionTaken = h.ActionTaken,
                Note = h.Note
            }).ToListAsync();

            return searchVa;
        }

        public async Task<MedicalEvent> UpdateMedicalEventAsync(UpdateMedicalEventRequest request)
        {
            var updateEvent = await _medicalEventRepository.GetById(request.EventId);
            if (updateEvent == null) throw AppExceptions.NotFoundId();

            updateEvent.NurseId = request.NurseId;
            updateEvent.EventDate = request.EventDate;
            updateEvent.Symptoms = request.Symptoms;
            updateEvent.ActionTaken = request.ActionTaken;
            updateEvent.Note = request.Note;

            await _medicalEventRepository.Update(updateEvent);
            return updateEvent;
        }

        public async Task<bool> DeleteMedicalEventAsync(int id)
        {
            try
            {
                var deleted = await _medicalEventRepository.GetById(id);
                if (deleted == null) throw AppExceptions.NotFoundId();

                await _medicalEventRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
