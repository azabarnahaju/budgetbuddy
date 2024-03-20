using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BudgetBuddy.Contracts;
﻿using BudgetBuddy.Contracts;
using BudgetBuddy.Contracts.Auth;
using BudgetBuddy.Contracts.Registration;
using BudgetBuddy.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(request.Email, request.Username, request.Password, "User");

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(Register), new {message = "Registration successful"});
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return Ok( new {message = "Login successful", data = new AuthResponse(result.Email, result.UserName, result.Token)});
    }
    
    [HttpPost("Validate")]
    public async Task<IActionResult> ValidateToken([FromBody] TokenValidationRequest request)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(request.Token) as JwtSecurityToken;
            if (token.ValidTo < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Token expired" });
            }
            
            var username = token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var email = token.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
            var role = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            
            return Ok(new { message = "Token is valid", data = new {username, email, userId, role } });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest(new { message = "Token validation failed" });
        }
    }

    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
}