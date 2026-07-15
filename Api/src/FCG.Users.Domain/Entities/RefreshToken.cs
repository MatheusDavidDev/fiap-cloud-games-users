using FCG.Users.Core.Models;

namespace FCG.Users.Domain.Entities;

public class RefreshToken: BaseEntity
{
    public RefreshToken(string token, DateTime expiresAt, Guid idUsuario)
    {
        Token = token;
        ExpiresAt = expiresAt;
        IdUsuario = idUsuario;
    }


    public Guid IdUsuario { get; private set; }
    public Usuario Usuario { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsActive() => !IsExpired() && !IsRevoked();

    public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked() => RevokedAt.HasValue;

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}
