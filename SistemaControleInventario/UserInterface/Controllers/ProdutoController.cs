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

        [HttpGet]
        public async Task<IActionResult> ListarProdutos()
        {
            return Ok(await _produtoService.ListarProdutos());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarPorId(int id)
        {
            return Ok(await _produtoService.ListarPorId(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, ProdutoRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _produtoService.AtualizarProduto(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            await _produtoService.DeletarProduto(id);
            return NoContent();
        }
    }
}
