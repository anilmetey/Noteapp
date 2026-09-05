using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotTrackApi.Data;
using NotTrackApi.DTOs;
using NotTrackApi.Models;
using NotTrackApi.Services;
using System.Security.Cryptography;
using System.Text;

namespace NotTrackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _tokenService;

        public AuthController(AppDbContext context, JwtTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Bu email zaten kayıtlı.");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Email = dto.Email,
                Name = dto.Name, 
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Kayıt başarılı!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Şifre yanlış.");

            var token = _tokenService.CreateToken(user);

           
            return Ok(new
            {
                token,
                email = user.Email,
                role = user.Role,
                name = user.Name
            });
        }

        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Me()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            return Ok($"Hoş geldin {email}");
        }
    }


}
