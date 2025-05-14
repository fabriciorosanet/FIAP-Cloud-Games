namespace FCG.Application.Authentication;

public sealed class JwtSettings
{
    public string Issuer           { get; init; } = default!;
    public string Audience         { get; init; } = default!;
    public string SigningKey       { get; init; } = default!;
    public int    ExpiresInMinutes { get; init; }
}