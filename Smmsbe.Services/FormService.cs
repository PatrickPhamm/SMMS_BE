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
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;
        private readonly IConsentFormRepository _consentFormRepository;
        private readonly IParentRepository _parentRepository;

        public FormService(IFormRepository formRepository
            , IConsentFormRepository consentFormRepository
            , IParentRepository parentRepository)
        {
            _formRepository = formRepository;
            _consentFormRepository = consentFormRepository;
            _parentRepository = parentRepository;
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

            return response;
        }

        public async Task<FormResponse> UpdateFormAsync(UpdateFormRequest request)
        {
            var updateForm = await _formRepository.GetById(request.FormId);

            if (updateForm == null) throw AppExceptions.NotFoundId();

            updateForm.Title = request.Title;
            updateForm.Content = request.Content;
            updateForm.ClassName = request.ClassName;

            await _formRepository.Update(updateForm);

            return new FormResponse
            {
                FormId = updateForm.FormId,
                Title = updateForm.Title,
                ClassName = updateForm.ClassName,
                Content = updateForm.Content,
                //SentDate = updateForm.SentDate,
                //CreatedAt= request.CreatedAt,
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
