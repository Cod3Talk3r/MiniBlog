namespace MiniBlog.Application.DTOs
{
    public record RegisterDto(string UserName, string Email, string Password);
    public record LoginDto(string Email, string Password);
    public record AuthResultDto(bool Success, string? Token, string? Error);
}