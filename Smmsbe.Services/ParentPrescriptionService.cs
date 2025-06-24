using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Common;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.Services
{
    public class ParentPrescriptionService : IParentPrescriptionService
    {
        private readonly IParentPrescriptionRepository _parentPrescriptionRepository;
        private readonly IParentRepository _parentRepository;
        private readonly AppSettings _appSettings;

        public ParentPrescriptionService(IParentPrescriptionRepository parentPrescriptionRepository
            , AppSettings appSettings, IParentRepository parentRepository)
        {
            _parentPrescriptionRepository = parentPrescriptionRepository;
            _appSettings = appSettings;
            _parentRepository = parentRepository;
        }

        public async Task<ParentPrescription> GetById(int id)
        {
            var entity = await _parentPrescriptionRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<List<ParentPrescriptionResponse2>> GetPrescriptionByParent(int parentId)
        {
            var entity = await _parentPrescriptionRepository.GetAll()
                                        .Include(x => x.Parent)      
                                        .Where(x => x.ParentId == parentId)
                                        .Select(x => new ParentPrescriptionResponse2
                                        {
                                            PrescriptionId = x.PrescriptionId,
                                            NurseId = x.NurseId,
                                            ParentNote = x.ParentNote,
                                            Schedule = x.Schedule,
                                            SubmittedDate = x.SubmittedDate,
                                            PrescriptionFile = x.PrescriptionFile,
                                            Parent = new ParentResponse
                                            {
                                                ParentId = x.Parent.ParentId,
                                                FullName = x.Parent.FullName,
                                                PhoneNumber = x.Parent.PhoneNumber,
                                                Email = x.Parent.Email,
                                                Address = x.Parent.Address
                                            },
                                        }).ToListAsync();

            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<ParentPrescription> AddParentPrescriptionAsync(AddParentPrescriptionRequest request)
        {
            var ParentPre = new ParentPrescription
            {
                NurseId = request.NurseId,
                SubmittedDate = request.SubmittedDate,
                Schedule = request.Schedule,
                ParentNote = request.ParentNote,
                ParentId = request.ParentId,
                PrescriptionFile = request.PrescriptionFile
            };

            return await _parentPrescriptionRepository.Insert(ParentPre);
        }

        public async Task<ParentPrescription> UpdateParentPrescriptionAsync(UpdateParentPrescriptionRequest request)
        {
            var updateParentPre = await _parentPrescriptionRepository.GetById(request.PrescriptionId);
            if(updateParentPre == null) throw AppExceptions.NotFoundId();

            updateParentPre.PrescriptionId = request.PrescriptionId;
            updateParentPre.Schedule = request.Schedule;
            updateParentPre.ParentNote = request.ParentNote;
            updateParentPre.PrescriptionFile = request.PrescriptionFile;

            await _parentPrescriptionRepository.Update(updateParentPre);
            return updateParentPre;
        }

        public async Task<List<ParentPrescriptionResponse>> SearchParentPrescriptionAsync(SearchParentPrescriptionRequest request)
        {
            var query = _parentPrescriptionRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.PrescriptionId.ToString().Contains(request.Keyword));

            var parPre = await query.Select(x => new ParentPrescriptionResponse
            {
                PrescriptionId = x.PrescriptionId,
                NurseId = x.NurseId,
                ParentId = x.ParentId,
                ParentNote = x.ParentNote,
                Schedule = x.Schedule,
                SubmittedDate = x.SubmittedDate,
                PrescriptionFile = x.PrescriptionFile
            }).ToListAsync();

            return parPre;
        }

        public async Task<bool> DeleteParentPrescriptionAsync(int id)
        {
            try
            {
                var parentPre = await _parentPrescriptionRepository.GetById(id);
                if (parentPre == null) throw AppExceptions.NotFoundId();

                await _parentPrescriptionRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        const string ImageFolder = "files/blogs/";

        public string GetImageFolder()
        {
            return ImageFolder;
        }

        private string GetImageUrl(string thumbnail)
        {
            if (string.IsNullOrEmpty(thumbnail)) return "";

            if (thumbnail.StartsWith("http://") || thumbnail.StartsWith("https://"))
                return thumbnail;

            // Assuming _appSettings.ApplicationUrl is the base URL of your application
            return $"{_appSettings.ApplicationUrl}/{ImageFolder}/{thumbnail}";
        }
    }
}
