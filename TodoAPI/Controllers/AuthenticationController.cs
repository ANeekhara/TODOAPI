using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TodoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthenticationController(IConfiguration config)
    {
        _config = config;

    }

    public record AuthenticationData(string? UserName, string? Password);
    public record UserData(int Id, string FirstName, string LastName, string UserName);


    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData authenticationData)
    {
        var user = ValidateCredentials(authenticationData);
        if (user is null)
        {
            return Unauthorized();
        }

        var token = GenerateToken(user);
        return Ok(token);
    }

    private string GenerateToken(UserData user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = new()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new (JwtRegisteredClaimNames.GivenName,user.FirstName),
            new (JwtRegisteredClaimNames.FamilyName,user.LastName),
        };

        var token = new JwtSecurityToken(_config.GetValue<string>("Authentication:Issuer"), _config.GetValue<string>("Authentication:Audience"),
            claims, DateTime.UtcNow,DateTime.UtcNow.AddMinutes(1),signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    private UserData? ValidateCredentials(AuthenticationData authenticationData)
    {
        //replace it with third party

        if (ComapareValues(authenticationData.UserName, "test") && ComapareValues(authenticationData.Password, "test1"))
        {
            return new UserData(1, "First Name", "Last Name", authenticationData.UserName);
        }

        return null;
    }

    private bool ComapareValues(string actual, string expected)
    {
        if (actual is not null)
        {
            if (actual.Equals(expected))
                return true;
        }
        return false;
    }
}