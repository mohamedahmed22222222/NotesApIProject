using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notes.Api.DTOUser;
using Notes.Api.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        UserManager<AppUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<AppUser> _userManager,IConfiguration config)

        {
            userManager = _userManager;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task< IActionResult> RegisterNewUser(DtoNewUser newUser) 
        {
            if (ModelState.IsValid) 
            {
                AppUser user = new()
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    PhoneNumber= newUser.Phone

                };
                IdentityResult result =  await userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    return Ok("success");

                }
                else 
                {
                    return BadRequest(result.Errors.Select(e=>e.Description));
                
                }

            
            }
            return BadRequest();
        
        
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(DtoLoginUser userDto) 
        {
            if (ModelState.IsValid) 
            {
                AppUser User = await userManager.FindByNameAsync(userDto.UserName);
                if (User != null) 
                {
                    if(await userManager.CheckPasswordAsync(User, userDto.Password)) // return bool
                    {
                        // Generate Token

                        List<Claim> UserClaims= new List<Claim>();
                        // change Jwt id  كل مره فيها لوجين
                        UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.Name, User.UserName));
                        UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));
                        var UserRoles= await userManager.GetRolesAsync(User);
                        foreach (var role in UserRoles) 
                        { 
                            UserClaims.Add(new Claim(ClaimTypes.Role, role));
                        
                        
                        }
                        var SignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
                        var signcer = new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256);



                        var MyToken = new JwtSecurityToken(
                            issuer: config["JWT:Issuer"],
                            audience: config["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            claims: UserClaims,
                            signingCredentials:signcer

                            
                            
                            
                            );
                        return Ok(new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                            Expiration = DateTime.Now.AddHours(1)// or MyToken.ValidTo
                        }



                            );
                    
                    }
                    else {
                        return Unauthorized();
                         }
                
                }
                else 
                {
                    ModelState.AddModelError("", "UserName Is Not Valid");
                }
            
            
            
            }
            return BadRequest(ModelState);
        
        
        }
    }
}
