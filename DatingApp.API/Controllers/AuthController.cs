using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dto;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")] // attribute routing 
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request 

            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();

            if (await _repo.UserExists(userForRegisterDto.UserName))
                return BadRequest("User already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.UserName
            };

            var userCreated = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserInfoLoginDto userInfoLoginDto)
        {
            var userFromRepo = await _repo.Login(userInfoLoginDto.UserName, userInfoLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();
            /// JWT Token payload 
            var claims = new[] {
                 new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                 new Claim(ClaimTypes.Name,userFromRepo.Username.ToString())
            };
            /// key 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            /// signin Credentials  is signature

            var signinCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var  tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signinCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
              token = tokenHandler.WriteToken(token)
            });



        }

    }
}