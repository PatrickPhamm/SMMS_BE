using Microsoft.EntityFrameworkCore;
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
    public class HealthProfileService : IHealthProfileService
    {
        private readonly IHealthProfileRepository _healthProfileRepository;

        public HealthProfileService(IHealthProfileRepository healthProfileRepository )
        {
            _healthProfileRepository = healthProfileRepository;
        }

        public async Task<HealthProfile> GetById(int id)
        {
            var entity = await _healthProfileRepository.GetById(id);
            if (entity == null) throw AppExceptions.NotFoundId();

            return entity;
        }

        public async Task<HealthProfile> AddHealthProfileAsync(AddHealthProfileRequest request)
        {
            var newHealthPro = new HealthProfile
            {
                StudentId = request.StudentId,
                BloodType = request.BloodType,
                Allergies = request.Allergies
            };

            return  await _healthProfileRepository.Insert(newHealthPro);
        }

        public async Task<List<HealthProfileResponse>> SearchHealthProfileAsync(SearchHealthProfileRequest request)
        {
            var query = _healthProfileRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.HealthProfileId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.StudentId.ToString()) && s.StudentId.ToString().Contains(request.Keyword)));

            var Hps = await query.Select(h => new HealthProfileResponse
            {
                HealthProfileId = h.HealthProfileId,
                StudentId= h.StudentId,
                BloodType = h.BloodType,
                Allergies = h.Allergies
            }).ToListAsync();

            return Hps;
        }

        public async Task<HealthProfile> UpdateHealthProfileAsync(UpdateHealthProfileRequest request)
        {
            var updateHealthProfile = await _healthProfileRepository.GetById(request.HealthProfileId);
            if (updateHealthProfile == null) throw AppExceptions.NotFoundId();

            updateHealthProfile.HealthProfileId = request.HealthProfileId; 
            updateHealthProfile.BloodType = request.BloodType;
            updateHealthProfile.Allergies = request.Allergies;

            await _healthProfileRepository.Update(updateHealthProfile);
            return updateHealthProfile;
        }

        public async Task<bool> DeleteHealthProfileAsync(int id)
        {
            try
            {
                var healthPro = await _healthProfileRepository.GetById(id);
                if (healthPro == null) throw AppExceptions.NotFoundId();

                await _healthProfileRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
