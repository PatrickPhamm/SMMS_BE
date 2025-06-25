using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services;
using Smmsbe.Services.Helpers;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using Smmsbe.WebApi.Helpers;

namespace Smmsbe.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/nurse")]
    [ApiController]
    public class nurseController : BaseAccountController
    {
        private readonly INurseService _nurseService;

        public nurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginNurseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _nurseService.AuthorizeAsync(request.Email, request.Password);

            return Ok(new
            {
                Success = true,
                User = new
                {
                    Id = acc.NurseId,
                    acc.Email,
                    acc.FullName,
                    AccessToken = AccessTokenGenerator.GenerateExpiringAccessToken(DateTime.Now.ToVNTime().AddDays(1))
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getById = await _nurseService.GetById(id);
            return Ok(getById);
        }

        #region getAll
        /*[HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _nurseService.GetAllAsync();
            return Ok(result);
        }*/
        #endregion

        [HttpPost("add")]
        public async Task<IActionResult> AddAccount([FromBody] AddNurseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _nurseService.AddNurseAsync(request);

            return Ok(new
            {
                account.NurseId,
                account.FullName,
                account.Username,
                account.Email
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccount(UpdateNurseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _nurseService.UpdateNurseAsync(request);

            return Ok(acc);
        }


        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchNurseRequest request)
        {
            var result = await _nurseService.SearchNurseAsync(request);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _nurseService.DeleteNurseAsync(id);

            return Ok();
        }

        [HttpGet("{id}/vaccineResults")]
        public async Task<IActionResult> GetVaccineResults(int id)
        {
            var result = await _nurseService.GetVaccinationResults(id);
            return Ok(result);
        }

        [HttpGet("{id}/healthCheckResults")]
        public async Task<IActionResult> GetHealthCheckResults(int id)
        {
            var result = await _nurseService.GetHealthCheckResults(id);
            return Ok(result);
        }

        [HttpPost("approve")]
        public async Task<IActionResult> Approve(int parentId)
        {
            var result = await _nurseService.ApproveNurseAsync(parentId);
            return Ok(new { success = result });
        }

        //Manager sẽ kích hoạt bằng tay
        /*[HttpPost("activate")]
        public async Task<IActionResult> Activate(ActivateRequest request)
        {
            var result = await _nurseService.ActivateAccountAsync(request.Code);
            return Ok(new { success = result });
        }*/

        //Nurse nhấn vào link để kích hoạt  
        /*[HttpGet("activate/{code}")]
        public async Task<IActionResult> ActivateNurse(string code)
        {
            var result = await _nurseService.ActivateAccountAsync(code);
            return Ok(new { success = result });
        }*/

        protected override async Task<bool> ActivateAccountAsync(string code)
        {
            return await _nurseService.ActivateAccountAsync(code);
        }
    }
}
