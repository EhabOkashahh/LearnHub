using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Domain.Exceptions.UnAuthorizeException;
using Microsoft.AspNetCore.Identity;
using ServicesAbstraction;
using Shared.DTOS.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Services
{
    public class AuthService(UserManager<AppUser> _userManger, IMapper _mapper) : IAuthService
    {
        public async Task<UserAuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManger.FindByEmailAsync(request.Email);
            if(user is null) throw new UserNotFoundException(request.Email);

            var authorize = await _userManger.CheckPasswordAsync(user,request.Password);
            if(!authorize) throw new UnAuthorizedException();

            return new UserAuthResponse(){
                DisplayName = user.DisplayName,
                Email = request.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        public async Task<UserAuthResponse> RegisterAsync(RegisterRequest request)
        {
            var user = _mapper.Map<AppUser>(request);

            var res = await _userManger.CreateAsync(user,request.Password);

            if(!res.Succeeded) throw new BadRequestException(string.Join(",",res.Errors.Select(E => E.Description)));

            return new UserAuthResponse()
            {
              DisplayName = request.DisplayName,
              Email = request.Email,
              Token = await GenerateTokenAsync(user),
            };
        }
    
        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TokenKey")!));


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!)
            };

            var roles = await _userManger.GetRolesAsync(user);


            foreach(var role in roles) claims.Add(new Claim(ClaimTypes.Role,role));
            


            var token = new JwtSecurityToken(
                issuer : Environment.GetEnvironmentVariable("API_BASE_URL"),
                audience: Environment.GetEnvironmentVariable("JwtAudiance"),
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(Environment.GetEnvironmentVariable("JwtDuration")!)),
                signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
    }
}