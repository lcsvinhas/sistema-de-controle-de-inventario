using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Application.Services;

namespace SistemaControleInventario.UserInterface.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;

        public LoginController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var token = await _authService.Autenticar(dto);
            return Ok(new { Token = token });
        }
    }
}
