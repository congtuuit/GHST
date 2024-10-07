using FluentValidation;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;

namespace GHSTShipping.Application.DTOs.Account.Requests
{
    public class AuthenticationRequest : IRequest<BaseResult<AuthenticationResponse>>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
    {
        public AuthenticationRequestValidator(ITranslator translator)
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .NotNull()
                .WithName(p => translator[nameof(p.UserName)]);

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                //.Matches(Regexs.Password)
                .WithName(p => translator[nameof(p.Password)]);
        }
    }
}
