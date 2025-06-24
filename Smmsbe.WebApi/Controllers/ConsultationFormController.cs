using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class consultationFormController : ControllerBase
    {
        private readonly IConsultationFormService _consultationFormService;
        public consultationFormController(IConsultationFormService consultationFormService)
        {
            _consultationFormService = consultationFormService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _consultationFormService.GetByIdAsync(id);
            return Ok(getById);
        }

        [HttpGet("getByParent")]
        public async Task<IActionResult> GetByParent(int parentId)
        {
            var list = await _consultationFormService.GetByParent(parentId);

            return Ok(list);
        }

        [HttpGet("getByStudent")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var list = await _consultationFormService.GetByStudent(studentId);

            return Ok(list);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddConsultationForm(AddConsultationFormRequest request)
        {
            var addConsultationForm = await _consultationFormService.AddConsultationFormAsync(request);
            return Ok(addConsultationForm);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchConsultationFormRequest request)
        {
            var result = await _consultationFormService.SearchConsultationFormAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateConsultationForm(UpdateConsultationFormRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateConsultationForm = await _consultationFormService.UpdateConsultationFormAsync(request);
            return Ok(updateConsultationForm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultationForm(int id)
        {
            var deleteConsultationForm = await _consultationFormService.DeleteConsultationFormAsync(id);
            return Ok();
        }

        [HttpPost("accept/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _consultationFormService.AcceptConsultation(id);

            return Ok(result);
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _consultationFormService.RejectConsultation(id);

            return Ok(result);
        }
    }
}
