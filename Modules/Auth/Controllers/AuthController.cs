using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.Auth.DTOs;
using warehouse_management_system.Modules.Auth.Interfaces;

namespace warehouse_management_system.Modules.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterDto dto)
        => Ok(await _service.RegisterAsync(dto));

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginDto dto)
    {
        var token = await _service.LoginAsync(dto);

        if (token == null)
            return Unauthorized();

        return Ok(token);
    }
}