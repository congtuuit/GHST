using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Helpers;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Infrastructure.Identity.Contexts;
using GHSTShipping.Infrastructure.Identity.Enums;
using GHSTShipping.Infrastructure.Identity.Models;
using GHSTShipping.Infrastructure.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Identity.Services
{
    public class AccountServices(
        IdentityContext identityContext,
        UserManager<ApplicationUser> userManager,
        IAuthenticatedUserService authenticatedUser,
        SignInManager<ApplicationUser> signInManager,
        JwtSettings jwtSettings, ITranslator translator) : IAccountServices
    {
        public async Task<BaseResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            var user = await userManager.FindByIdAsync(authenticatedUser.UserId);

            var token = await userManager.GeneratePasswordResetTokenAsync(user!);
            var identityResult = await userManager.ResetPasswordAsync(user, token, model.Password);

            if (identityResult.Succeeded)
                return BaseResult.Ok();

            return identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)).ToList();
        }

        public async Task<BaseResult> ChangeUserNameAsync(ChangeUserNameRequest model)
        {
            var user = await userManager.FindByIdAsync(authenticatedUser.UserId);

            user.UserName = model.UserName;

            var identityResult = await userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
                return BaseResult.Ok();

            return identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)).ToList();
        }

        public async Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
        {
            var user = await userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.AccountMessages.Account_NotFound_with_UserName(request.UserName)), nameof(request.UserName));
            }

            var signInResult = await signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return new Error(ErrorCode.FieldDataInvalid, translator.GetString(TranslatorMessages.AccountMessages.Invalid_password()), nameof(request.Password));
            }

            return await GetAuthenticationResponse(user);
        }

        public async Task<BaseResult<AuthenticationResponse>> AuthenticateByUserNameAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.AccountMessages.Account_NotFound_with_UserName(username)), nameof(username));
            }

            return await GetAuthenticationResponse(user);
        }

        public async Task<BaseResult<string>> RegisterGhostAccountAsync()
        {
            var user = new ApplicationUser()
            {
                UserName = GenerateRandomString(7),
                Type = "ADMIN",
                Name = GenerateRandomString(7),
            };

            var identityResult = await userManager.CreateAsync(user);

            if (identityResult.Succeeded)
                return user.UserName;

            return identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)).ToList();

            string GenerateRandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                var random = new Random();
                return new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }

        public async Task<BaseResult<UserDto>> CreateAccountAsync(CreateAccountRequest request)
        {
            var user = new ApplicationUser()
            {
                Type = AccountTypeConstants.SHOP,
                UserName = request.Email,
                Name = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            var existedUser = await userManager.FindByEmailAsync(user.Email);
            if (existedUser != null)
            {
                return new Error(ErrorCode.Duplicated, "Email address already existed");
            }

            var existedPhone = await identityContext.Users.AnyAsync(i => i.PhoneNumber == request.PhoneNumber);
            if (existedPhone)
            {
                return new Error(ErrorCode.Duplicated, "Phone number already existed");
            }

            string password = GeneratePassword(6);
            await userManager.CreateAsync(user, password);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                SecurityStamp = user.SecurityStamp,
            };
        }

        private static string GeneratePassword(int length)
        {
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_-+=<>?";

            string allChars = lowerCase + upperCase + digits + specialChars;
            Random random = new Random();
            StringBuilder password = new StringBuilder();

            // Ensure at least one character from each category
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Fill the rest of the password length with random characters from all categories
            for (int i = password.Length; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the characters in the password to ensure randomness
            return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
        }

        private async Task<AuthenticationResponse> GetAuthenticationResponse(ApplicationUser user)
        {
            await userManager.UpdateSecurityStampAsync(user);

            var jwToken = await GenerateJwtToken();

            var rolesList = await userManager.GetRolesAsync(user);

            return new AuthenticationResponse()
            {
                Id = user.Id.ToString(),
                JwToken = new JwtSecurityTokenHandler().WriteToken(jwToken),
                Email = user.Email,
                UserName = user.UserName,
                Roles = rolesList,
                IsVerified = user.EmailConfirmed,
            };

            async Task<JwtSecurityToken> GenerateJwtToken()
            {
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                return new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: (await signInManager.ClaimsFactory.CreateAsync(user)).Claims,
                    expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
                    signingCredentials: signingCredentials);
            }
        }
    }
}
