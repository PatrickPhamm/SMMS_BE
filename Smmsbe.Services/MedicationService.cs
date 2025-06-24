using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _medicationRepository;
        private readonly IParentPrescriptionRepository _parentPrescriptionRepository;

        public MedicationService(IMedicationRepository medicationRepository, IParentPrescriptionRepository parentPrescriptionRepository)
        {
            _medicationRepository = medicationRepository;
            _parentPrescriptionRepository = parentPrescriptionRepository;
        }

        public async Task<Medication> GetById(int id)
        {
            var entity = await _medicationRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<MedicationResponse> GetByIdAsync(int id)
        {
            var entity = await _medicationRepository.GetAll()
                                                        .Include(x => x.Prescription)
                                                        .Where(x => x.MedicationId == id)
                                                        .Select(x => new MedicationResponse
                                                        {
                                                            MedicationId = x.MedicationId,
                                                            StudentId = x.StudentId,
                                                            MedicationName = x.MedicationName,
                                                            Dosage = x.Dosage,
                                                            Quantity = x.Quantity,
                                                            RemainingQuantity = x.RemainingQuantity,
                                                            Prescription = new ParentPrescriptionResponse
                                                            {
                                                                PrescriptionId = x.Prescription.PrescriptionId,
                                                                NurseId = x.Prescription.NurseId,
                                                                ParentId = x.Prescription.ParentId,
                                                                SubmittedDate = x.Prescription.SubmittedDate,
                                                                Schedule = x.Prescription.Schedule,
                                                                ParentNote = x.Prescription.ParentNote,
                                                                PrescriptionFile = x.Prescription.PrescriptionFile
                                                            }, 
                                                        }).FirstOrDefaultAsync();

            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<List<MedicationResponse>> GetMedicalByPrescription(int prescriptionId)
        {
            var entity = await _medicationRepository.GetAll()
                                                        .Include(x => x.Prescription)
                                                        .Where(x => x.PrescriptionId == prescriptionId)
                                                        .Select(x => new MedicationResponse
                                                        {
                                                            MedicationId = x.MedicationId,
                                                            StudentId = x.StudentId,
                                                            MedicationName = x.MedicationName,
                                                            Dosage = x.Dosage,
                                                            Quantity = x.Quantity,
                                                            RemainingQuantity = x.RemainingQuantity,
                                                            Prescription = new ParentPrescriptionResponse
                                                            {
                                                                PrescriptionId = x.Prescription.PrescriptionId,
                                                                NurseId = x.Prescription.NurseId,
                                                                ParentId = x.Prescription.ParentId,
                                                                SubmittedDate = x.Prescription.SubmittedDate,
                                                                Schedule = x.Prescription.Schedule,
                                                                ParentNote = x.Prescription.ParentNote,
                                                                PrescriptionFile = x.Prescription.PrescriptionFile
                                                            },
                                                        }).ToListAsync();

            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<List<MedicationByStudentResponse>> GetMedicalByStudent(int studentId)
        {
            return await _medicationRepository.GetAll()
                .Include(x => x.Prescription)
                .Where(x => x.StudentId == studentId)
                .Select(x => new MedicationByStudentResponse
                {
                    MedicationId = x.MedicationId,
                    MedicationName = x.MedicationName,
                    Dosage = x.Dosage,
                    Quantity = x.Quantity,
                    RemainingQuantity = x.RemainingQuantity,
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

        public async Task<Medication> AddMedicationAsync(AddMedicationRequest request)
        {
            var newMedication = new Medication()
            {
                StudentId = request.StudentId,
                MedicationName = request.MedicationName,
                PrescriptionId = request.PrescriptionId,
                Dosage = request.Dosage,
                Quantity = request.Quantity,
                RemainingQuantity = request.RemainingQuantity
            }; 

            return await _medicationRepository.Insert(newMedication);
        }

        public async Task<List<MedicationResponse>> SearchMedicationAsync(SearchMedicationRequest request)
        {
            var query = _medicationRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.MedicationId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.MedicationName) && s.MedicationName.Contains(request.Keyword)));

            var searchMe = await query.Select(x => new MedicationResponse
            {
                MedicationId = x.MedicationId,
                MedicationName = x.MedicationName,
                Dosage= x.Dosage,
                Quantity = x.Quantity,  
                RemainingQuantity = x.RemainingQuantity,
                Prescription = new ParentPrescriptionResponse
                {
                    PrescriptionId = x.Prescription.PrescriptionId,
                    NurseId = x.Prescription.NurseId,
                    ParentId = x.Prescription.ParentId,
                    SubmittedDate = x.Prescription.SubmittedDate,
                    Schedule = x.Prescription.Schedule,
                    ParentNote = x.Prescription.ParentNote,
                    PrescriptionFile = x.Prescription.PrescriptionFile
                }
            }).ToListAsync();

            return searchMe;
        }

        public async Task<Medication> UpdateMedicationAsync(UpdateMedicationRequest request)
        {
            var updateMedication = await _medicationRepository.GetById(request.MedicationId);
            if (updateMedication == null) throw AppExceptions.NotFoundId();

            updateMedication.MedicationName = request.MedicationName;
            updateMedication.Dosage = request.Dosage;
            updateMedication.Quantity = request.Quantity;
            updateMedication.RemainingQuantity = request.RemainingQuantity;

            await _medicationRepository.Update(updateMedication);
            return updateMedication;
        }

        public async Task<bool> DeleteMedicationAsync(int id)
        {
            try
            {
                var deleted = await _medicationRepository.GetById(id);
                if (deleted == null) throw AppExceptions.NotFoundId();

                await _medicationRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
