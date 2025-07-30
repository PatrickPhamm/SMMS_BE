using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services.Common;
using Smmsbe.Services.Enum;
using Smmsbe.Services.Exceptions;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.Services
{
    public class FormService : IFormService
    {
        private readonly ILogger _logger;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IConsentFormRepository _consentFormRepository;
        private readonly IFormRepository _formRepository;
        private readonly IVaccinationResultRepository _vaccinationResultRepository;
        private readonly IHealthCheckResultRepository _healthCheckResultRepository;
        private readonly IVaccinationScheduleRepository _vaccinationScheduleRepository;
        private readonly IHealthCheckupScheduleRepository _healthCheckupScheduleRepository;
        private readonly IHashHelper _hashHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly AppSettings _appSettings;

        public FormService(IConsentFormRepository consentFormRepository,
            IParentRepository parentRepository,
            IStudentRepository studentRepository,
            IFormRepository formRepository,
            IVaccinationResultRepository vaccinationResultRepository,
            IHealthCheckResultRepository healthCheckResultRepository,
            IHashHelper hashHelper,
            IEmailHelper emailHelper,
            AppSettings appSettings,
            IVaccinationScheduleRepository vaccinationScheduleRepository,
            IHealthCheckupScheduleRepository healthCheckupScheduleRepository,
            ILogger<FormService> logger) 
        {
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _consentFormRepository = consentFormRepository;
            _formRepository = formRepository;
            _vaccinationResultRepository = vaccinationResultRepository;
            _healthCheckResultRepository = healthCheckResultRepository;
            _hashHelper = hashHelper;
            _emailHelper = emailHelper;
            _appSettings = appSettings;
            _vaccinationScheduleRepository = vaccinationScheduleRepository;
            _healthCheckupScheduleRepository = healthCheckupScheduleRepository;
            _logger = logger;
        }

        public async Task<FormResponse> GetById(int id)
        {
            var entity = await _formRepository.GetById(id);
            if(entity == null) throw AppExceptions.NotFoundId();

            return new FormResponse
            {
                FormId = entity.FormId,
                Title = entity.Title,
                ClassName = entity.ClassName,
                Content = entity.Content,
                SentDate = entity.SentDate,
                CreatedAt = entity.CreatedAt,
                Type = ((FormType)entity.Type).ToString()
            };
        }

        #region Tạo Form riêng
        /*public async Task<FormResponse> AddFormAsync(AddFormRequest request)
        {
            var newForm = new Form
            {
                ClassName = request.ClassName,
                Title = request.Title,
                Content = request.Content,
                SentDate = request.SentDate,
                CreatedAt = request.CreatedAt,
                Type = (int)request.Type
            };

            var newEntity = await _formRepository.Insert(newForm);

            return new FormResponse
            {
                FormId = newForm.FormId,
                Title = newForm.Title,
                ClassName = newForm.ClassName,
                Content = newForm.Content,
                SentDate = newForm.SentDate,
                CreatedAt = newForm.CreatedAt,
                Type = ((FormType)newForm.Type).ToString()
            };
        }*/
        #endregion

        //Tạo Form mới thì ConsentForm sẽ được tạo tự động tạo ra cùng lúc
        public async Task<FormResponseAdded> AddFormAsync(AddFormRequest request)
        {
            // Kiểm tra tất cả ParentId có tồn tại không
            foreach (var parentId in request.ParentIds)
            {
                var parentExists = await _parentRepository.ParentIdExsistAsync(parentId);
                if (!parentExists)
                {
                    throw AppExceptions.NotFoundId();
                }
            }

            // Tạo Form mới
            var newForm = new Form
            {
                ManagerId = 1,
                ClassName = request.ClassName,
                Title = request.Title,
                Content = request.Content,
                SentDate = request.SentDate,
                CreatedAt = request.CreatedAt ?? DateTime.Now,
                Type = (int)request.Type
            };

            var createdForm = await _formRepository.Insert(newForm);

            // Tạo ConsentForm cho mỗi ParentId
            var consentForms = new List<ConsentForm>();
            foreach (var parentId in request.ParentIds)
            {
                var consentForm = new ConsentForm
                {
                    FormId = createdForm.FormId,
                    ParentId = parentId,
                    Status = (int)ConsentFormStatus.Pending 
                };

                var createdConsentForm = await _consentFormRepository.Insert(consentForm);
                consentForms.Add(createdConsentForm);
            }

            // Trả về FormResponse với thông tin ConsentForm
            var response = new FormResponseAdded
            {
                FormId = createdForm.FormId,
                Title = createdForm.Title,
                ClassName = createdForm.ClassName,
                Content = createdForm.Content,
                SentDate = createdForm.SentDate,
                CreatedAt = createdForm.CreatedAt,
                Type = ((FormType)createdForm.Type).ToString(),
                ConsentForms = consentForms.Select(cf => new AddConsentFormResponse
                {
                    ConsentFormId = cf.ConsentFormId,
                    ParentId = cf.ParentId,
                    Status = ((ConsentFormStatus)cf.Status).ToString()
                }).ToList()
            };

            // Gửi email thông báo cho từng phụ huynh
            await SendFormNotificationEmails(request.ParentIds, createdForm);

            return response;
        }

        private async Task SendFormNotificationEmails(List<int> parentIds, Form form)
        {
            foreach (var parentId in parentIds)
            {
                try
                {
                    var parent = await _parentRepository.GetAll()
                        .Include(p => p.Students)
                        .FirstOrDefaultAsync(p => p.ParentId == parentId);

                    if (parent == null) continue;

                    // Lấy TẤT CẢ con của phụ huynh (không lọc theo lớp)
                    var allStudents = parent.Students.ToList();

                    if (!allStudents.Any())
                    {
                        _logger.LogWarning("No students found for Parent ID {ParentId}", parentId);
                        continue;
                    }

                    // Tạo danh sách tên tất cả các con
                    //var studentNames = string.Join(", ", allStudents.Select(s => s.FullName));

                    // Tạo danh sách tên và mã số tất cả các con
                    var studentNames = string.Join(", ", allStudents.Select(s => $"{s.FullName} - {s.StudentNumber}"));

                    var emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", "FormNotificationEmail.html");
                    var htmlContent = await File.ReadAllTextAsync(emailTemplatePath);

                    var content = htmlContent.Replace("{{StudentName}}", studentNames)
                                             .Replace("{{ParentName}}", parent.FullName ?? "N/A")
                                             .Replace("{{ParentEmail}}", parent.Email ?? "N/A")
                                             .Replace("{{FormTitle}}", form.Title ?? "N/A")
                                             .Replace("{{FormContent}}", form.Content ?? "N/A")
                                             .Replace("{{CreatedDate}}", DateTime.Now.ToString("dd/MM/yyyy"))
                                             .Replace("{{SystemUrl}}", _appSettings.LandingPageUrl ?? "#");   //# : FE điền link vào.

                    await _emailHelper.SendEmailAsync(parent.Email, $"Thông báo: {form.Title}", content, true, null, null);

                    _logger.LogInformation("Sent notification email to {Email} for all {StudentCount} students: {StudentNames}",
                        parent.Email, allStudents.Count, studentNames);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending email to Parent ID {ParentId}", parentId);
                }
            }
        }

        public async Task<UpdateFormResponse> UpdateFormAsync(UpdateFormRequest request)
        {
            var updateForm = await _formRepository.GetById(request.FormId);

            if (updateForm == null) throw AppExceptions.NotFoundId();

            updateForm.Title = request.Title;
            updateForm.Content = request.Content;
            updateForm.ClassName = request.ClassName;

            await _formRepository.Update(updateForm);

            return new UpdateFormResponse
            {
                FormId = updateForm.FormId,
                Title = updateForm.Title,
                ClassName = updateForm.ClassName,
                Content = updateForm.Content,
                Type = ((FormType)updateForm.Type).ToString()
            };
        }

        public async Task<List<FormResponse>> SearchFormAsync(SearchFormRequest request)
        {
            var query = _formRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(
                            s => s.FormId.ToString().Contains(request.Keyword) ||
                            (!string.IsNullOrEmpty(s.ClassName) && s.ClassName.Contains(request.Keyword)));

            var forms = await query.Select(n => new FormResponse
            {
                FormId = n.FormId,
                Title = n.Title,
                Content = n.Content,
                ClassName = n.ClassName,
                SentDate= n.SentDate,
                CreatedAt = n.CreatedAt,
                Type = ((FormType)n.Type).ToString()
            }).ToListAsync();

            return forms;
        }

        public async Task<bool> DeleteFormAsync(int id)
        {
            try
            {
                var deleteForm = await _formRepository.GetById(id);

                if (deleteForm == null) throw AppExceptions.NotFoundId();

                await _formRepository.Delete(id);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
