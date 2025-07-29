 using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Common;
using Smmsbe.Services.Enum;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Helpers;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;


namespace Smmsbe.Services
{
    public class NurseService : INurseService
    {
        private readonly ILogger _logger;
        private readonly INurseRepository _nurseRepository;
        private readonly IVaccinationResultRepository _vaccinationResultRepository;
        private readonly IHealthCheckResultRepository _healthCheckResultRepository;
        private readonly IHashHelper _hashHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly AppSettings _appSettings;

        public NurseService(INurseRepository nurseRepository, IVaccinationResultRepository vaccinationResultRepository
                , IHealthCheckResultRepository healthCheckResultRepository, IHashHelper hashHelper
                , IEmailHelper emailHelper, AppSettings appSettings, ILogger<ParentService> logger)
        {
            _nurseRepository = nurseRepository;
            _vaccinationResultRepository = vaccinationResultRepository;
            _healthCheckResultRepository = healthCheckResultRepository;
            _hashHelper = hashHelper;
            _emailHelper = emailHelper;
            _appSettings = appSettings;
            _logger = logger;
        }

        public async Task<Nurse> GetById(int id)
        {
            var nurse = await _nurseRepository.GetById(id);

            return nurse;
        }

        #region getAll
        /*public async Task<List<NurseResponse>> GetAllAsync()
        {
            var nurses = await _nurseRepository.GetAll()
                                        .OrderBy(x => x.FullName)
                                        .Select(x => new NurseResponse
                                        {
                                            NurseId = x.NurseId,
                                            FullName = x.FullName,
                                            PhoneNumber = x.PhoneNumber,
                                            Email = x.Email
                                        })
                                        .ToListAsync();

            return nurses;
        }*/
        #endregion

        public async Task<Nurse> AddNurseAsync(AddNurseRequest request)
        {
            /*if (string.IsNullOrEmpty(request.Username) || request.Username.Length < 4)
                throw AppExceptions.BadRequestUsernameIsInvalid();*/

            // Check if email already exists
            var exstingAcc = await _nurseRepository.GetAll().FirstOrDefaultAsync(x => x.Email == request.Email);
            if (exstingAcc != null) throw AppExceptions.BadRequestEmailIsExists();

            var activationCode = Guid.NewGuid().ToString("N");

            var newNurseAcc = new Nurse
            {
                FullName = request.FullName,
                Username = request.Username,
                PasswordHash = _hashHelper.HashPassword(request.PasswordHash),
                Email = request.Email,
                IsActive = false,
                ActivationCode = activationCode
            };

            //return await _nurseRepository.Insert(newNurseAcc);

            var nurse = await _nurseRepository.Insert(newNurseAcc);

            // Gửi mail chúc mừng khách hàng đã tạo tài khoản
            try
            {
                var emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", "AccountCreatedEmail.html");

                var htmlContent = await File.ReadAllTextAsync(emailTemplatePath);

                var content = htmlContent.Replace("{{UserName}}", nurse.FullName)
                                         .Replace("{{UserEmail}}", nurse.Email)
                                         .Replace("{{CreatedDate}}", DateTime.Now.ToVNTime().ToString("dd/MM/yyyy"))
                                         //.Replace("{{ActivationLink}}", $"{_appSettings.ApplicationUrl}/api/nurse/activate/{activationCode}");   //chỉ test trên BE
                                         .Replace("{{ActivationLink}}", $"{_appSettings.LandingPageUrl}/activate/{activationCode}");    //kết nối với FE

                _logger.LogInformation("Sending account created email to {Email}", nurse.Email);

                await _emailHelper.SendEmailAsync(nurse.Email, "Tài khoản đã được tạo thành công", content, true, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending account created email.");
            }

            return nurse;
        }

        public async Task<Nurse> AuthorizeAsync(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) 
                throw AppExceptions.NotFoundAccount();

            var accNur = await _nurseRepository.GetAll().SingleOrDefaultAsync(x => x.Email == email);
            if(accNur == null) throw AppExceptions.NotFoundAccount();

            if (accNur.IsActive == false) throw AppExceptions.AccountNotActivated();  // Chưa kích hoạt tài khoản

            var passwordHash = _hashHelper.HashPassword(password);

            if(accNur.PasswordHash != passwordHash) throw AppExceptions.NotFoundAccount();

            return accNur;
        }

        public async Task<NurseResponse> UpdateNurseAsync(UpdateNurseRequest request)
        {
            var updateNurse = await _nurseRepository.GetById(request.NurseId);
            if(updateNurse == null) throw AppExceptions.NotFoundAccount();

            
            updateNurse.FullName = request.FullName;
            updateNurse.Username = request.Username;
            updateNurse.Email = request.Email;
            
            await _nurseRepository.Update(updateNurse);
            return new NurseResponse
            {
                NurseId = updateNurse.NurseId,
                FullName = updateNurse.FullName,
                Username = updateNurse.Username,
                Email = updateNurse.Email
            };
        }

        public async Task<List<NurseResponse>> SearchNurseAsync(SearchNurseRequest request)
        {
            var query = _nurseRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.NurseId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.FullName) && s.FullName.Contains(request.Keyword)) ||
                            (!string.IsNullOrEmpty(s.Username) && s.Username.Contains(request.Keyword)));

            var nurses = await query.Select(n => new NurseResponse
            {
                NurseId = n.NurseId,
                FullName = n.FullName,
                Username = n.Username,
                Email = n.Email
            }).ToListAsync();

            return nurses;
        }

        public async Task<bool> DeleteNurseAsync(int id)
        {
            try
            {
                var acc = await _nurseRepository.GetById(id);
                if (acc == null) throw AppExceptions.NotFoundAccount();

                await _nurseRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<VaccinationResultResponse>> GetVaccinationResults(int nurseId)
        {
            var getVac = await _vaccinationResultRepository.GetAll()
                                                .Where(x => x.NurseId == nurseId)
                                                .Select(x => new VaccinationResultResponse
                                                {
                                                    VaccinationResultId = x.VaccinationResultId,
                                                    VaccinationScheduleId = x.VaccinationScheduleId,
                                                    HealthProfileId = x.HealthProfileId,
                                                    NurseId = x.NurseId,
                                                    NurseName = $"{x.Nurse.FullName}",
                                                    Status = ((ResultStatus)x.Status).ToString(),
                                                    DoseNumber = x.DoseNumber,
                                                    Note = x.Note
                                                })
                                                .ToListAsync();
            return getVac;
        }

        public async Task<List<HealthCheckResultResponse>> GetHealthCheckResults(int id)
        {
            var getHea = await _healthCheckResultRepository.GetAll()
                                                .Where(x => x.NurseId == id)
                                                .Select(x => new HealthCheckResultResponse
                                                {
                                                    HealthCheckupRecordId = x.HealthCheckupRecordId,
                                                    HealthCheckScheduleId = x.HealthCheckScheduleId,
                                                    HealthProfileId = x.HealthProfileId,
                                                    NurseId = x.NurseId,
                                                    NurseName = $"{x.Nurse.FullName}",
                                                    Status = ((ResultStatus)x.Status).ToString(),
                                                    Height = x.Height,
                                                    Weight = x.Weight,
                                                    LeftVision = x.LeftVision,
                                                    RightVision = x.RightVision,
                                                    Result = x.Result,
                                                    Note = x.Note

                                                })
                                                .ToListAsync();
            return getHea;
        }

        public async Task<bool> ApproveNurseAsync(int nurseId)
        {
            var nurse = await _nurseRepository.GetById(nurseId);

            if (nurse == null) throw AppExceptions.NotFoundAccount();

            nurse.IsActive = true;

            nurse.Note = "Tài khoản đã được kích hoạt bởi manager";

            await _nurseRepository.Update(nurse);

            try
            {
                var emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", "AccountActivatedEmail.html");

                var htmlContent = await File.ReadAllTextAsync(emailTemplatePath);

                var content = htmlContent.Replace("{{UserFullName}}", $"{nurse.FullName}")
                                         .Replace("{{ActivatedTime}}", DateTime.Now.ToVNTime().ToString("dd/MM/yyyy HH:mm:ss"));

                _logger.LogInformation("Sending activation email to {Email}", nurse.Email);

                await _emailHelper.SendEmailAsync(nurse.Email, "Tài khoản đã kích hoạt", content, true, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending activation email.");
            }

            return true;
        }

        public async Task<bool> ActivateAccountAsync(string code)
        {
            var nurse = await _nurseRepository.GetAll().FirstOrDefaultAsync(x => x.ActivationCode == code);

            if (nurse == null || nurse.IsActive) throw AppExceptions.BadRequest("Tài khoản đã được kích hoạt trước đó.");

            nurse.IsActive = true;
            nurse.ActivationCode = null;
            nurse.Note = "Tài khoản đã được kích hoạt.";

            await _nurseRepository.Update(nurse);

            try
            {
                var emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", "AccountActivatedEmail.html");

                var htmlContent = await File.ReadAllTextAsync(emailTemplatePath);

                var content = htmlContent.Replace("{{UserFullName}}", $"{nurse.FullName}")
                                         .Replace("{{ActivatedTime}}", DateTime.Now.ToVNTime().ToString("dd/MM/yyyy HH:mm:ss"));

                _logger.LogInformation("Sending account activation email to {Email}", nurse.Email);

                await _emailHelper.SendEmailAsync(nurse.Email, "Tài khoản của bạn đã được kích hoạt", content, true, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending account activation email.");
            }

            return true;
        }
    }
}
