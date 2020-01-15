using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WeightLossTracker.Api.Helpers;
using WeightLossTracker.DataStore.DTOs;
using WeightLossTracker.DataStore.DTOs.Content;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Repositories.Impl;

namespace WeightLossTracker.Api.Controllers
{
    [Route("api/members")]
    [ApiController]
    [Authorize]
    public class MembersController : Controller
    {
        private MemberRepository _memberManager;
        private readonly SignInManager<UserProfileModel> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public MembersController(MemberRepository memberManager,
            SignInManager<UserProfileModel> signInManager, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _memberManager = memberManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        /// <summary>
        /// Get Token to authorise session
        /// </summary>
        /// <param name="userdto">The username and password for login</param>
        /// <returns>a jwt token to authorise session</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] UserDto userdto)
        {
            var user = await _memberManager.FindByEmailAsync(userdto.username);

            if (user == null)
            {
                return Json("User Does not Exist");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userdto.password, false);

            if (!result.Succeeded)
            {
                return Json( "Invalid Password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),

                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            await _memberManager.UpdateAsync(user);

            
            return Json(tokenHandler.WriteToken(token));
        }

        [HttpGet("{id}")]
        public async Task <ActionResult<UserProfileDto>> GetProfileInformation(string id)
        {
            var memberfromRepo = await _memberManager.FindByIdAsync(id);

            if(memberfromRepo == null)
            {
                return NotFound();
            }

            var member = _mapper.Map<UserProfileDto>(memberfromRepo);

            return Ok(member);

        }
    }
}