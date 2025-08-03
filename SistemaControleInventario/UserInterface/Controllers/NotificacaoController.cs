using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaControleInventario.Application.Services;

namespace SistemaControleInventario.UserInterface.Controllers
{
    [Route("notificacoes")]
    [ApiController]
    [Authorize]
    public class NotificacaoController : ControllerBase
    {
        private readonly NotificacaoService _service;

        public NotificacaoController(NotificacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ListarNotificacoes()
        {
            return Ok(await _service.ListarNotificacoes());
        }

        [HttpPut("{id}/marcar-como-lida")]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            await _service.MarcarComoLida(id);
            return NoContent();
        }
    }
}
