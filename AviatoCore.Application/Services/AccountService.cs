﻿using AviatoCore.Application.DTOs;
using AviatoCore.Application.Interfaces;
using AviatoCore.Domain.Entities;
using AviatoCore.Infrastructure;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AviatoCore.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AviatoDbContext _context;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, AviatoDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IdentityResult> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Email,
                Name = registerDto.Name,
                Surname = registerDto.Surname
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Client");
                if (!roleResult.Succeeded)
                {
                    return IdentityResult.Failed(roleResult.Errors.ToArray());
                }

                var client = new Client
                {
                    ClientId = user.Id,
                    UserId = user.Id,
                    Country = registerDto.Country,
                    ClientTypeId = 1
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IdentityResult> AddWorker(WorkerDto workerDto)
        {
            var user = new User
            {
                UserName = workerDto.Email,
                Name = workerDto.Name,
                Surname = workerDto.Surname
            };

            var result = await _userManager.CreateAsync(user, workerDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, workerDto.Role);
                if (!roleResult.Succeeded)
                {
                    return IdentityResult.Failed(roleResult.Errors.ToArray());
                }

                if (workerDto.Role != "Admin")
                {
                    var worker = new Worker
                    {
                        WorkerId = user.Id,
                        UserId = user.Id,
                        AirportId = workerDto.AirportId
                    };

                    _context.Workers.Add(worker);
                    await _context.SaveChangesAsync();
                }
            }

            return result;
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_secret_key_here_that_is_at_least_32_characters_long"); // Replace with your secret key
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName ?? string.Empty) };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration, adjust as needed
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResult> Login(LoginDto loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginDto.Email);
                var roles = await _userManager.GetRolesAsync(user);
                var token = await GenerateJwtToken(user);

                return new LoginResult
                {
                    Token = token,
                    Role = roles[0]
                };
            }

            return null;
        }
    }
}
