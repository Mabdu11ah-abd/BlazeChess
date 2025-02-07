using chessApp.server.services;
using ChessApp.commonModels;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IMongoCollection<UserModel> _users;

    public AuthController(IConfiguration config, MongoDbContext database)
    {
        _config = config;
        _users = database.Users;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Validate user
        var user = await _users.Find(u => u.Username == request.Username).FirstOrDefaultAsync();
        if (user == null || request.Password !=  user.PasswordHash)
            return Unauthorized(new
            {
                Message = "Invalid username or password",
                RequestData = request,  // Include the request data
                UserData = user          // Include user data (null if user not found)
            });

        // Generate tokens
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user.Id);

        return Ok(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        // Decrypt and validate refresh token
        var (userId, isValid) = ValidateRefreshToken(request.RefreshToken);
        if (!isValid) return Unauthorized(
            new
            {
                Message = "Invalid username or password 1",
                RequestData = request,  // Include the request data
                UserData = userId         // Include user data (null if user not found)
            });

        // Get user
        var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        if (user == null) return Unauthorized(new
        {
            Message = "Invalid username or password",
            RequestData = request,  // Include the request data
            UserData = userId         // Include user data (null if user not found)
        });

        // Generate new tokens
        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken(user.Id);

        return Ok(new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    private string GenerateAccessToken(UserModel user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:AccessTokenExpirationMinutes")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(string userId)
    {
        var payload = new
        {
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenExpirationDays"))
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: new[] { new Claim(JwtRegisteredClaimNames.Sub, userId) },
            expires: payload.ExpiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private (string UserId, bool IsValid) ValidateRefreshToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]))
            };
            

            var principal = handler.ValidateToken(token, validationParameters, out _);
            
            var claims = principal.Claims.ToDictionary(c => c.Type, c => c.Value);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return (userId, true);
        }
        catch
        {
            return (null, false);
        }
    }
}
public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
public class RefreshRequest
{
    public string RefreshToken { get; set; }
}
public class LoginRequest
{
    public string Username { get; set; } // Ensure this matches the JSON field
    public string Password { get; set; }
}