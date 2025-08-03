using Microsoft.IdentityModel.Tokens;
using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Application.Exceptions;
using SistemaControleInventario.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaControleInventario.Application.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _config;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _config = config;
        }

        public async Task<string> Autenticar(LoginRequestDTO dto)
        {
            var usuario = await _usuarioRepository.BuscarPorEmail(dto.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha))
            {
                throw new AuthException("Usuário ou senha inválidos");
            }

            var perfis = usuario.UsuarioPerfis?.Select(p => p.Perfil.Nome).ToList() ?? new List<string>();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

            claims.AddRange(perfis.Select(perfil => new Claim(ClaimTypes.Role, perfil)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
