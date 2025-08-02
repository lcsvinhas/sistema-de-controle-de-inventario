using Microsoft.AspNetCore.Mvc;
using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Application.Services;

namespace SistemaControleInventario.UserInterface.Controllers
{
    [Route("produtos")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarProduto([FromBody] ProdutoRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _produtoService.AdicionarProduto(dto));
        }
    }
}
