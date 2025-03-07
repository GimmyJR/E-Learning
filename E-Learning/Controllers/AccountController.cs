﻿using E_Learning.Dto;
using E_Learning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = new ApplicationUser
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                AvatarUrl = registerUser.AvatarUrl,
                Bio = registerUser.Bio,
                DateJoined = DateTime.Now
            };
            var result = await userManager.CreateAsync(user,registerUser.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User Registered successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await userManager.FindByEmailAsync(loginUser.Email);
            if (user == null) return Unauthorized();
            var result = await signInManager.CheckPasswordSignInAsync(user,loginUser.Password,false);
            if (result.Succeeded)
            {
                var token = GenerateToken(user);
                return Ok(new {Token = token});
            }
            return BadRequest();
        }

        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin(string provider,string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties (provider, redirectUrl);
            return Challenge(properties,provider);
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null,string remoteError = null)
        {
            if(remoteError !=null )
            {
                return BadRequest(new { mMessage = $"Error from external provider: {remoteError}" });
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest(new { mMessage = "Error loading external login information." });
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await userManager.FindByLoginAsync(info.LoginProvider,info.ProviderKey);
                var token = GenerateToken(user);
                return Ok(new { Token = token});
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if(email != null)
            {
                var user = new ApplicationUser { UserName = email, Email = email ,DateJoined = DateTime.Now};
                var identityresult = await userManager.CreateAsync(user);
                if(identityresult.Succeeded)
                {
                    await userManager.AddLoginAsync(user, info);
                    var token = GenerateToken(user);
                    return Ok(new { Token = token });
                }
            }
            return BadRequest(new { Message = "Failed to authenticate." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new { Message = "User logged out successfully" });
        }

        private string GenerateToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
