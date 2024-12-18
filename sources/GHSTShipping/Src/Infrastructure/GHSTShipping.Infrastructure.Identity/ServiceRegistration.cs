using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Infrastructure.Identity.Contexts;
using GHSTShipping.Infrastructure.Identity.Models;
using GHSTShipping.Infrastructure.Identity.Services;
using GHSTShipping.Infrastructure.Identity.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GHSTShipping.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration, bool UseInMemoryDatabase)
        {
            if (UseInMemoryDatabase)
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase(nameof(IdentityContext)));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"), o => o.UseCompatibilityLevel(120)));
            }

            services.AddTransient<IGetUserServices, GetUserServices>();
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IUserSessionService, UserSessionService>();

            var identitySettings = configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();

            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            services.AddSingleton(jwtSettings);

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = false;

                options.Password.RequireDigit = identitySettings.PasswordRequireDigit;
                options.Password.RequiredLength = identitySettings.PasswordRequiredLength;
                options.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireNonAlphanumeric;
                options.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
                options.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;

            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30); // Set a validation interval that suits your app's needs
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsJsonAsync(BaseResult.Failure(new Error(ErrorCode.AccessDenied, "You are not Authorized")));
                        },
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = 403;
                            await context.Response.WriteAsJsonAsync(BaseResult.Failure(new Error(ErrorCode.AccessDenied, "You are not authorized to access this resource")));
                        },
                        OnTokenValidated = async context =>
                        {
                            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
                            if (claimsIdentity?.Claims.Any() is not true)
                                context.Fail("This token has no claims.");

                            var allowMultiDeviceLogin = configuration.GetSection("allowMultiDeviceLogin").Get<bool>();
                            if (allowMultiDeviceLogin)
                            {
                                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                                var userSessionService = context.HttpContext.RequestServices.GetRequiredService<IUserSessionService>();

                                // Validate that the session for this token exists
                                var sessionIsValid = await userSessionService.ValidateUserSessionAsync(Guid.Parse(userId), context.HttpContext.Connection.RemoteIpAddress?.ToString());
                                if (!sessionIsValid)
                                {
                                    context.Fail("Session is not valid.");
                                }
                            }
                            else
                            {
                                var securityStamp = claimsIdentity?.FindFirst("AspNet.Identity.SecurityStamp");
                                if (securityStamp is null)
                                    context.Fail("This token has no security stamp");

                                var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
                                var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                                if (validatedUser is null)
                                    context.Fail("Token security stamp is not valid.");
                            }

                        },

                    };
                });

            return services;
        }
    }
}
