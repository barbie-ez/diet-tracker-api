using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WeightLossTracker.Api.Helpers;
using WeightLossTracker.DataStore.DTOs;
using WeightLossTracker.DataStore.Entitties;

namespace WeightLossTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private UserManager<UserProfileModel> _userManager;
        private readonly SignInManager<UserProfileModel> _signInManager;
        private readonly AppSettings _appSettings;

        public AccountController(UserManager<UserProfileModel> userManager,
            SignInManager<UserProfileModel> signInManager, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] UserDto userdto)
        {
            var user = await _userManager.FindByEmailAsync(userdto.username);

            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userdto.password, false);

            if (!result.Succeeded)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            await _userManager.UpdateAsync(user);

            return Json(token);
        }
    }
}