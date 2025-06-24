using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Common;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Helpers;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.Services
{
    public class ParentService : IParentService
    {
        private readonly ILogger _logger;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IHashHelper _hashHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly AppSettings _appSettings;

        public ParentService(ILogger<ParentService> logger, IParentRepository parentRepository
            , IStudentRepository studentRepository, IHashHelper hashHelper
            , IEmailHelper emailHelper, AppSettings appSettings)
        {
            _logger = logger;
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _hashHelper = hashHelper;
            _emailHelper = emailHelper;
            _appSettings = appSettings;
        }

        public async Task<Parent> GetById(int id)
        {
            var parent = await _parentRepository.GetById(id);
            return parent;
        }

        /*public async Task<List<ParentResponse>> GetAllAsync()
        {
            var parents = await _parentRepository.GetAll()
                                        .OrderBy(x => x.LastName)
                                        .Select(x => new ParentResponse
                                        {
                                            ParentId = x.ParentId,
                                            Fullname = x.Fullname,
                                            PhoneNumber = x.PhoneNumber,
                                            Email = x.Email
                                        })
                                        .ToListAsync();

            return parents;
        }*/

        public async Task<List<StudentResponse>> GetParentFromStudent(int studentId)
        {
            var students = await _studentRepository.GetAll()
                                                     .Where(x => x.StudentId == studentId)
                                                     .Select(x => new StudentResponse
                                                     {
                                                         StudentId = studentId,
                                                         FullName = x.FullName,
                                                         DateOfBirth = x.DateOfBirth,
                                                         ClassName = x.ClassName,
                                                         Gender = x.Gender,
                                                         StudentNumber = x.StudentNumber,
                                                         Parent = new ParentResponse
                                                         {
                                                             ParentId = x.Parent.ParentId,
                                                             FullName = x.Parent.FullName,
                                                             PhoneNumber = x.Parent.PhoneNumber,
                                                             Email = x.Parent.Email,
                                                             Address = x.Parent.Address
                                                         }
                                                     }).ToListAsync();

            return students;
        }

        public async Task<Parent> AddParentAsync(AddParentRequest request)
        {
            /*if (string.IsNullOrEmpty(request.Username) || request.Username.Length < 4)
                throw AppExceptions.BadRequestUsernameIsInvalid();*/

            // Check if email already exists
            var exstingAcc = await _parentRepository.GetAll().FirstOrDefaultAsync(x => x.Email == request.Email);
            if (exstingAcc != null) throw AppExceptions.BadRequestEmailIsExists();

            var activationCode = Guid.NewGuid().ToString("N");

            var newParentAcc = new Parent
            {
                FullName = request.FullName,
                PasswordHash = _hashHelper.HashPassword(request.PasswordHash),
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Address = request.Address,
                IsActive = false,
                ActivationCode = activationCode
            };

            //return await _parentRepository.Insert(newParentAcc);

            var parent = await _parentRepository.Insert(newParentAcc);

            // Gửi mail chúc mừng khách hàng đã tạo tài khoản
            try
            {
                var emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", "AccountCreatedEmail.html");

                var htmlContent = await File.ReadAllTextAsync(emailTemplatePath);

                var content = htmlContent.Replace("{{UserName}}", parent.FullName)
                                         .Replace("{{UserEmail}}", parent.Email)
                                         .Replace("{{CreatedDate}}", DateTime.Now.ToVNTime().ToString("dd/MM/yyyy"))
                                         //.Replace("{{ActivationLink}}", $"{_appSettings.LandingPageUrl}/activate/{activationCode}");
                                         .Replace("{{ActivationLink}}", $"{_appSettings.LandingPageUrl}/api/parent/activate/{activationCode}");

                _logger.LogInformation("Sending account created email to {Email}", parent.Email);

                await _emailHelper.SendEmailAsync(parent.Email, "Tài khoản đã được tạo thành công", content, true, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending account created email.");
            }

            return parent;
        }

        //Authorize dành cho đăng nhập bằng mail
        #region
        public async Task<Parent> AuthorizeAsync(string email, string password)
        {
            //if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                //throw AppExceptions.NotFoundAccount();

            var accPar = await _parentRepository.GetAll().SingleOrDefaultAsync(x => x.Email == email);

            if (accPar == null) throw AppExceptions.NotFoundAccount();

            if (accPar.IsActive == false) throw AppExceptions.AccountNotActivated();

            var passwordHash = _hashHelper.HashPassword(password);

            if (accPar.PasswordHash != passwordHash) throw AppExceptions.NotFoundAccount();

            return accPar;
        }
        #endregion

        //Authorize dành cho đăng nhập bình thường
        #region
        /*public async Task<Parent> AuthorizeAsync(string email, string password)
        {
           //if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
           //    throw AppExceptions.NotFoundAccount();

           var accPar = await _parentRepository.GetAll().SingleOrDefaultAsync(x => x.Email == email);

           if (accPar == null) throw AppExceptions.NotFoundAccount();

           var passwordHash = _hashHelper.HashPassword(password);

           if (accPar.PasswordHash != passwordHash) throw AppExceptions.NotFoundAccount();

           return accPar;
        }*/
        #endregion

        public async Task<Parent> UpdateParentAsync(UpdateParentRequest request)
        {
            var updateParent = await _parentRepository.GetById(request.ParentId);
            if (updateParent == null) throw AppExceptions.NotFoundAccount();

            updateParent.FullName = request.FullName;
            updateParent.PhoneNumber = request.PhoneNumber;
            updateParent.Email = request.Email;
            updateParent.Address = request.Address;

            await _parentRepository.Update(updateParent);
            return updateParent;
        }

        public async Task<List<ParentResponse>> SearchParentAsync(SearchParentRequest request)
        {
            var query = _parentRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.ParentId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.FullName) && s.FullName.Contains(request.Keyword)) ||
                            (!string.IsNullOrEmpty(s.PhoneNumber) && s.PhoneNumber.Contains(request.Keyword)));

            var parents = await query.Select(n => new ParentResponse
            {
                ParentId = n.ParentId,
                FullName = n.FullName,
                PhoneNumber = n.PhoneNumber,
                Email = n.Email,
                Address = n.Address
            }).ToListAsync();

            return parents;
        }

        public async Task<bool> DeleteParentAsync(int id)
        {
            try
            {
                var acc = await _parentRepository.GetById(id);
                if (acc == null) throw AppExceptions.NotFoundAccount();

                await _parentRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ApproveParentAsync(int parentId)
        {
            var parent = await _parentRepository.GetById(parentId);

            if (parent == null) throw AppExceptions.NotFoundAccount();

            parent.IsActive = true;

            parent.Note = "Tài khoản đã được kích hoạt bởi manager";

            await _parentRepository.Update(parent);

            try
            {
                var emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", "AccountActivatedEmail.html");

                var htmlContent = await File.ReadAllTextAsync(emailTemplatePath);

                var content = htmlContent.Replace("{{UserFullName}}", $"{parent.FullName}")
                                         .Replace("{{ActivatedTime}}", DateTime.Now.ToVNTime().ToString("dd/MM/yyyy HH:mm:ss"));

                _logger.LogInformation("Sending activation email to {Email}", parent.Email);

                await _emailHelper.SendEmailAsync(parent.Email, "Tài khoản đã kích hoạt", content, true, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending activation email.");
            }

            return true;
        }

        public async Task<bool> ActivateAccountAsync(string code)
        {
            var parent = await _parentRepository.GetAll().FirstOrDefaultAsync(x => x.ActivationCode == code);

            if (parent == null || parent.IsActive) throw AppExceptions.BadRequest("Tài khoản đã được kích hoạt trước đó.");

            parent.IsActive = true;
            parent.ActivationCode = null;
            parent.Note = "Tài khoản đã được kích hoạt.";

            await _parentRepository.Update(parent);

            try
            {
                var emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", "AccountActivatedEmail.html");

                var htmlContent = await File.ReadAllTextAsync(emailTemplatePath);

                var content = htmlContent.Replace("{{UserFullName}}", $"{parent.FullName}")
                                         .Replace("{{ActivatedTime}}", DateTime.Now.ToVNTime().ToString("dd/MM/yyyy HH:mm:ss"));

                _logger.LogInformation("Sending account activation email to {Email}", parent.Email);

                await _emailHelper.SendEmailAsync(parent.Email, "Tài khoản của bạn đã được kích hoạt", content, true, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending account activation email.");
            }

            return true;
        }
    }
}
