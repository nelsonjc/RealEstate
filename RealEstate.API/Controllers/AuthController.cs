using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Core.Constants;
using RealEstate.Core.DTOs;
using RealEstate.Core.Exceptions;
using RealEstate.Core.Interfaces.Services;
using System.Net;

namespace RealEstate.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthDto auth)
        {
            var response = await _service.Authenticate(auth);

            if (response.User != null) 
            {
                var token = await _service.GenerateToken(response.User);  
                return Ok(new { token });
            }
            else
            {
                throw new BusinessException(HttpStatusCode.BadRequest, MessageConstant.AUTHENTICATION_ERROR, response.Message);
            }
        }        
    }
}
