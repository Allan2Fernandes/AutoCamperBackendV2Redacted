using AutoCamperBackendV2.DataTransferObjects;
using AutoCamperBackendV2.Functions;
using AutoCamperBackendV2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoCamperBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ParkInPeaceProjectContext context;
        UserServices Services;
        // Copied this from appsettings. Find a better way to align them
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);
        private readonly IConfiguration _config;

        public UserController(ParkInPeaceProjectContext context, IConfiguration config) 
        { 
            this.context = context;
            _config = config;
            this.Services = new UserServices(this.context);
        }


        [AllowAnonymous]
        [HttpPost(nameof(CreateUser))]
        public async Task<ActionResult<string>> CreateUser(TblUser NewUser)
        {
            //Check if the email already exists in the database
            var queriedUsers = Services.GetUserOnEmail(NewUser.FldEmail);

            if(!Functions.IsListempty(queriedUsers))
            {
                return Ok("Email Exists");
            }            

            Services.AddUserToDatabase(NewUser);
            return Ok("User Created");
        }

        [AllowAnonymous]
        [HttpPost(nameof(LoginUser))]
        public async Task<ActionResult<string>> LoginUser(UserLoginDTO UserToLogin)
        {

            var user = Services.FindSingleUserOnEmailAndPassword(UserToLogin);

            if (user == null)
            {
                return Ok(new { Result =  "Account Not Found"});
            }
            else
            {            
                // Turn this into a function to generate a token
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim("UserID", user.FldUserId.ToString()),
                new Claim("Name",UserToLogin.FldEmail),
                new Claim("Role", (bool)user.FldIsAdmin?"Admin":"User")
                };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials);

                return Ok(new { Result = "Account Found", token = new JwtSecurityTokenHandler().WriteToken(token) });        
            }
        }

        [HttpGet(nameof(GetUserDetails) + "/{UserID}")]
        public async Task<ActionResult<TblUser>> GetUserDetails(int UserID)
        {
            var user = Services.FindUserOnUserID(UserID);
            if (user == null)
            {
                return Ok(new { Result = "Account Not found" });
            }
            else
            {
                return Ok( new {Result = "Account Found",  FoundUser = user});
            }
        }

        [HttpPost(nameof(UpdateUserDetails))]
        public async Task<ActionResult<string>> UpdateUserDetails(UpdateUserDetailsDTO UserDetails)
        {
            var resultMessage = Services.UpdateUserDetailsOnUserID(UserDetails);
            return Ok(resultMessage);
        } 
    }
}
