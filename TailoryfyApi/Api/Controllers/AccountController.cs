using Api.Extensions;
using Core.Models;
using Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private IPasswordHasher<AppUser> _hasher;
        private IConfiguration _config;
        private IEmailClient _emailClient;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> hasher,
            IConfiguration config, IEmailClient emailClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hasher = hasher;
            _config = config;
            _emailClient = emailClient;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    CustomerTypeId = model.CustomerTypeId
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(model);
                }
            }
            return BadRequest("Failed to create account");
        }

        [HttpPut("{userName}")]
        [Authorize]
        public async Task<IActionResult> Put(string userName, [FromBody]UserProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userName);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.CustomerTypeId = model.CustomerTypeId;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return NoContent();
                }
            }
            return BadRequest("Failed to create account");
        }

        [HttpPost("OneTimePassword/{userName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOneTimePassword(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var password = Security.GeneratePassword(8);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                {
                    return await SendOtpCode(userName, password);
                }
            }
            else
            {
                return await SendOtpCode(userName, password);
            }
            return BadRequest("Failed to generate password");
        }



        private async Task<IActionResult> SendOtpCode(string userName, string password)
        {
            var body = $"Dear {userName}, your OTP is {password}";
            await _emailClient.SendEmailAsync(userName, "Your OTP password", body);
            return Ok(new
            {
                IsOtpSent = true
            });
        }

        [HttpPost("api/auth/login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest("Failed to login");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);

                    var claims = new[]
                    {
                          new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                          new Claim(JwtRegisteredClaimNames.Email, user.Email),
                          new Claim(JwtRegisteredClaimNames.Sid, user.Id)
                        }.Union(userClaims);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                       issuer: _config["Tokens:Issuer"],
                       audience: _config["Tokens:Audience"],
                       claims: claims,
                       expires: DateTime.UtcNow.AddYears(1),
                       signingCredentials: creds
                       );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            else
            {
                return BadRequest("No user found!");
            }
            return BadRequest("Failed to generate token");
        }
    }
}
