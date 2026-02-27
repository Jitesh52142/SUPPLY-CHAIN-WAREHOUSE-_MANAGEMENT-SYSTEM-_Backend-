namespace warehouse_management_system.Modules.Auth.DTOs;

public class LoginDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}