using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smmsbe.Services.Helpers;
using Smmsbe.Services.Interfaces;
using Smmsbe.Services.Models;
using Smmsbe.WebApi.Helpers;

namespace Smmsbe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class managerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public managerController(IManagerService managerService)
        {
            _managerService = managerService;   
        }

        #region register
        /* [HttpPost("Register")]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterManagerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _managerService.RegisterManagerAsync(request);

            return Ok(new ManagerResponse
            {
                ManagerId = account.ManagerId,
                Email = account.Email,
                FullName = account.FullName,
                PhoneNumber = account.PhoneNumber
            });
        }*/
        #endregion

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] LoginManagerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _managerService.AuthorizeAsync(request.Email, request.Password);

            return Ok(new
            {
                Success = true,
                User = new
                {
                    Id = acc?.ManagerId,
                    acc?.Email,
                    Role = "manager",
                    AccessToken = AccessTokenGenerator.GenerateExpiringAccessToken(DateTime.Now.ToVNTime().AddDays(1))
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getId = await _managerService.GetById(id);
            return Ok(getId);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateManagerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var acc = await _managerService.UpdateManagerAsync(request);

            return Ok(acc);
        }
    }
}
