using AutoCamperBackendV2.Functions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AutoCamperBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        [HttpGet(nameof(CheckIfOnline))]
        public async Task<ActionResult<string>> CheckIfOnline()
        { 
               return Ok("Online");
        }

    }
}
