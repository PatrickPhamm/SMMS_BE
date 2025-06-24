using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class medicationController : ControllerBase
    {
        private readonly IMedicationService _medicationService;

        public medicationController(IMedicationService medicationService)
        {
            _medicationService = medicationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _medicationService.GetByIdAsync(id);
            return Ok(getById);
        }

        [HttpGet("getMedicalByPrescription")]
        public async Task<IActionResult> GetMedicalByParent(int prescriptionId)
        {
            var list = await _medicationService.GetMedicalByPrescription(prescriptionId);

            return Ok(list);
        }

        [HttpGet("getMedicalByStudent")]
        public async Task<IActionResult> GetMedicalByStudent(int studentId)
        {
            var list = await _medicationService.GetMedicalByStudent(studentId);

            return Ok(list);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMedication(AddMedicationRequest request)
        {
            var addMedication = await _medicationService.AddMedicationAsync(request);
            return Ok(addMedication);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchMedicationRequest request)
        {
            var result = await _medicationService.SearchMedicationAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMedication(UpdateMedicationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateMedication = await _medicationService.UpdateMedicationAsync(request);
            return Ok(updateMedication);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var deleteMedication = await _medicationService.DeleteMedicationAsync(id);
            return Ok();
        }
    }
}
