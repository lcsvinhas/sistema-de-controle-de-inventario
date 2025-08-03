using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Application.Services;

namespace SistemaControleInventario.UserInterface.Controllers
{
    [Route("usuarios")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] UsuarioRequestDTO dto)
        {
            return Ok(await _service.CriarUsuario(dto));
        }
    }
}
