using warehouse_management_system.Modules.Auth.DTOs;

namespace warehouse_management_system.Modules.Auth.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);

    Task<string?> LoginAsync(LoginDto dto);
}