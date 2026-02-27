using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Auth.Entities;

public class AppUser : BaseEntity
{
    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public string Role { get; set; } = "User";
}