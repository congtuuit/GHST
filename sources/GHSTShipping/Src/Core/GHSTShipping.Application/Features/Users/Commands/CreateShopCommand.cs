using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Users.Commands
{
    public class CreateShopCommand : CreateAccountRequest, IRequest<BaseResult<Guid>>
    {
        public string ShopName { get; set; }

        public decimal AvgMonthlyYield { get; set; }

        public string BankName { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankAccountHolder { get; set; }

        public string BankAddress { get; set; }
    }

    public class CreateShopCommandHandler(
        IShopRepository shopRepository,
        IUnitOfWork unitOfWork,
        IAccountServices accountServices,
        ITranslator translator,
        IEmailSender emailSender,
        ILogger<CreateShopCommandHandler> _logger
        ) : IRequestHandler<CreateShopCommand, BaseResult<Guid>>
    {
        public async Task<BaseResult<Guid>> Handle(CreateShopCommand request, CancellationToken cancellationToken)
        {
            using var createShopTransaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var createAccount = await accountServices.CreateAccountAsync(new CreateAccountRequest
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                });

                if (createAccount.Success)
                {
                    Domain.Entities.Shop shop = new(
                        createAccount.Data.Id,
                        request.FullName,
                        request.PhoneNumber,
                        request.AvgMonthlyYield,
                        DateTime.UtcNow)
                    {
                        BankName = request.BankName,
                        BankAddress = request.BankAddress,
                        BankAccountHolder = request.BankAccountHolder,
                        BankAccountNumber = request.BankAccountNumber
                    };

                    await shopRepository.AddAsync(shop);
                    await unitOfWork.SaveChangesAsync();

                    await createShopTransaction.CommitAsync(cancellationToken);

                    var sendEmail = await emailSender.SendEmailSetPasswordAsync(
                        createAccount.Data.Email,
                        createAccount.Data.Name,
                        createAccount.Data.SecurityStamp);

                    if (!sendEmail.Success)
                    {
                        _logger.LogInformation("Send email failed: {Errors}", sendEmail.Errors);
                    }

                    return shop.Id;
                }

                await createShopTransaction.RollbackAsync(cancellationToken);

                return createAccount.Errors;
            }
            catch (Exception ex)
            {
                await createShopTransaction.RollbackAsync(cancellationToken);

                _logger.LogError(ex, ex.Message);

                return new Error(ErrorCode.Exception, ex.Message);
            }
        }
    }
}
