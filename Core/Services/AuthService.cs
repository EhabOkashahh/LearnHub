using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Domain.Exceptions.UnAuthorizeException;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ServicesAbstraction.Auth;
using Shared.DTOS.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Domain.Contracts;

namespace Services
{
    public class AuthService(UserManager<AppUser> _userManger, IMapper _mapper, IOptions<JwtOptions> _jwtOptions, IUnitOfWork _uof) : IAuthService
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

        public async Task<UserAuthResponse> RegisterAsync(RegisterRequest request,CancellationToken ct)
        {
            var user = _mapper.Map<AppUser>(request);

            var res = await _userManger.CreateAsync(user,request.Password);

            if(!res.Succeeded) throw new BadRequestException(string.Join(",",res.Errors.Select(E => E.Description)));

            user.StudentProfile = new StudentProfile(){Id = user.Id};
                       
            await _uof.SaveChangesAsync(ct);
 
            return new UserAuthResponse()
            {
              DisplayName = request.DisplayName,
              Email = request.Email,
              Token = await GenerateTokenAsync(user),
            };
        }
    
        public async Task<string> GenerateTokenAsync(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.TokenKey));

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!)
            };

            var roles = await _userManger.GetRolesAsync(user);

            foreach(var role in roles) claims.Add(new Claim(ClaimTypes.Role,role));

            var token = new JwtSecurityToken(
                issuer : _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtOptions.Value.DurationInDays),
                signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
    }
}