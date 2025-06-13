using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;
using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public AuthService(IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<SignUpResponseDto> SignUp(User user)
        {
            user.Id = Guid.NewGuid();
            AppUser appUser = _mapper.Map<AppUser>(user);
            IdentityResult result = await _userManager.CreateAsync(appUser,user.Password);

            return new SignUpResponseDto {SignUpSuccessfully = result.Succeeded ? true : false,UserId = user.Id};
        }
    }
}