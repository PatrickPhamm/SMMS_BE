using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Common;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class parentPrescriptionController : ControllerBase
    {
        private readonly IParentPrescriptionService _parentPrescriptionService;
        private readonly IImageService _imageService;
        private readonly AppSettings _appSettings;

        public parentPrescriptionController(IParentPrescriptionService parentPrescriptionService, IImageService imageService,
            AppSettings appSettings)
        {
            _parentPrescriptionService = parentPrescriptionService;
            _imageService = imageService;
            _appSettings = appSettings;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _parentPrescriptionService.GetById(id);
            return Ok(getById);
        }

        [HttpGet("getPrescriptionByParent")]
        public async Task<IActionResult> GetMedicalByParent(int parentId)
        {
            var list = await _parentPrescriptionService.GetPrescriptionByParent(parentId);

            return Ok(list);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddParentPrescription(AddParentPrescriptionRequest request)
        {
            var addParentPrescription = await _parentPrescriptionService.AddParentPrescriptionAsync(request);
            return Ok(addParentPrescription);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchParentPrescriptionRequest request)
        {
            var result = await _parentPrescriptionService.SearchParentPrescriptionAsync(request);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateParentPrescription(UpdateParentPrescriptionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateParentPrescription = await _parentPrescriptionService.UpdateParentPrescriptionAsync(request);
            return Ok(updateParentPrescription);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentPrescription(int id)
        {
            var deleteParentPrescription = await _parentPrescriptionService.DeleteParentPrescriptionAsync(id);
            return Ok();
        }

        // <summary>
        /// Max 5MB, Filetypes: ".jpg", ".jpeg", ".png", ".gif"
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadParentPreImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded");

            // Validate file is an image
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Invalid file type. Only jpg, jpeg, png, and gif are allowed.");

            // Size validation (e.g., max 5MB)
            if (imageFile.Length > 5 * 1024 * 1024)
                return BadRequest("File size exceeds the limit of 5MB.");

            try
            {
                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var folder = _parentPrescriptionService.GetImageFolder();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);
                var result = await _imageService.UploadImageAsync(filePath, imageFile.OpenReadStream());

                return Ok(result ? new
                {
                    PathFull = $"{_appSettings.ApplicationUrl}/{folder}/{fileName}",
                    FileName = fileName
                } : null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
