using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Helpers;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using System.Text.Json;

namespace Smmsbe.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IHashHelper _hashHelper;

        public ManagerService(IManagerRepository managerRepository, IHashHelper hashHelper)
        {
            _managerRepository = managerRepository;
            _hashHelper = hashHelper;
        }

        #region register
        /*public async Task<Manager> RegisterManagerAsync(RegisterManagerRequest request)
        {

            var newAcc = new Manager
            {
                PasswordHash = _hashHelper.HashPassword(request.PasswordHash),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
            };

            var mana =  await _managerRepository.Insert(newAcc);
            return mana;
        }*/
        #endregion

        public async Task<Manager> GetById(int id)
        {
            var manager = await _managerRepository.GetById(id);

            if (manager == null) throw AppExceptions.NotFoundId();

            return manager;
        }

        public async Task<Manager> AuthorizeAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw AppExceptions.BadRequestEmailIsInvalid();

            var acc = await _managerRepository.GetAll().SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if (acc == null)
                throw AppExceptions.NotFoundAccount();

            var passwordHash = _hashHelper.HashPassword(password);

            if (acc.PasswordHash != passwordHash)
                throw AppExceptions.NotFoundAccount();

            return acc;
        }

        public async Task<Manager> UpdateManagerAsync(UpdateManagerRequest request)
        {
            var updateAcc = await _managerRepository.GetById(request.ManagerId);

            if (updateAcc == null) throw AppExceptions.NotFoundAccount();

            updateAcc.ManagerId = request.ManagerId;
            updateAcc.FullName = request.FullName;
            updateAcc.PhoneNumber = request.PhoneNumber;
            updateAcc.Email = request.Email;

            await _managerRepository.Update(updateAcc);

            return updateAcc;
        }
    }
}
