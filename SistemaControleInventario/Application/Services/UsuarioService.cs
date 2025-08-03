using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Application.Exceptions;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;

namespace SistemaControleInventario.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponseDTO> CriarUsuario(UsuarioRequestDTO dto)
        {
            var usuarioExistente = await _usuarioRepository.BuscarPorEmail(dto.Email);

            if (usuarioExistente != null)
            {
                throw new UsuarioException("Já existe um usuário com este e-mail.");
            }

            var usuario = new Usuario(dto.Nome, dto.Email, dto.Senha);

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            await _usuarioRepository.Save(usuario);

            await _usuarioRepository.AddPerfisAoUsuario(usuario.Id, dto.Perfis);

            return new UsuarioResponseDTO(usuario.Id, usuario.Nome, usuario.Email);
        }
    }
}
