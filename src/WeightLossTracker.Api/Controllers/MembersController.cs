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
using WeightLossTrackeData.Repositories.Impl;
using WeightLossTracker.Api.Helpers;
using WeightLossTrackerData.Constants;
using WeightLossTrackerData.DTOs.Content;
using WeightLossTrackerData.DTOs.Creation;
using WeightLossTrackerData.Entities;

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
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpMember( UserCreationDTO user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserProfileModel newUser = new UserProfileModel();
                    newUser.Email = newUser.UserName = user.Email;
                    newUser.FirstName = user.FirstName;
                    newUser.LastName = user.LastName;
                    newUser.Height = user.Height;
                    newUser.CurrentWeight = user.CurrentWeight;
                    newUser.DateOfBirth = new DateTimeOffset(Convert.ToDateTime(user.DateOfBirth));
                    newUser.PhoneNumber = user.PhoneNumber;

                    var result = _memberManager.CreateAsync(newUser, user.Password).GetAwaiter().GetResult();
                    var token = _memberManager.GenerateEmailConfirmationTokenAsync(newUser).GetAwaiter().GetResult();
                    var confirmEmail = _memberManager.ConfirmEmailAsync(newUser, token).GetAwaiter().GetResult();
                    if (confirmEmail.Succeeded)
                    {
                        if (result.Succeeded)
                        {
                            var newResult = _memberManager.AddToRoleAsync(newUser, AppRoles.Member).GetAwaiter().GetResult();
                            if (newResult.Succeeded)
                            {
                                return Json("User created Successfully");
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            return Json("ModelState not valid");
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