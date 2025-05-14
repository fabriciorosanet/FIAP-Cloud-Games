using FCG.Domain.Usuarios.Entities;

namespace FCG.Application.Authentication.Interface;

public interface ITokenService
{
    (string token, DateTime expiresAt) GenerateToken(Usuario usuario);
}