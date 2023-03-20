﻿using LMS.Shared.ResponseModel;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.API.Security
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ResponseModel<LoginResponse> GenerateToken(ResponseModel<LoginResponse> userLoginDetails)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userLoginDetails.Data.UserId.ToString()),
                new Claim(ClaimTypes.Role, userLoginDetails.Data.RoleType)
            };
            string newToken = string.Empty;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var tokenValidTime = Convert.ToInt16(_configuration.GetSection("Jwt:Expiry").Value);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(null, null, claims.ToArray(), expires: DateTime.Now.AddDays(tokenValidTime), signingCredentials: credentials);
            newToken = new JwtSecurityTokenHandler().WriteToken(token);
            if(newToken != string.Empty)
            {
                userLoginDetails.IsSuccess = true;
                userLoginDetails.Message = "Login Successfull and Token Generated.";
                userLoginDetails.Data.Token = newToken;
            }
            else
            {
                userLoginDetails.IsSuccess = false;
                userLoginDetails.Message = "Login Successfull but Token Generation failed.";
            }
            return userLoginDetails;

        }
    }
}