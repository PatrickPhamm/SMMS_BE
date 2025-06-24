using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Smmsbe.WebApi.Controllers
{
    public abstract class BaseAccountController : ControllerBase
    {
        protected abstract Task<bool> ActivateAccountAsync(string code);

        [HttpGet("activate/{code}")]
        public async Task<IActionResult> Activate(string code)
        {
            var result = await ActivateAccountAsync(code);
            return Ok(new { success = result });
        }
    }
}
