using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
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
        ITranslator translator
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

                    return shop.Id;
                }

                await createShopTransaction.RollbackAsync(cancellationToken);

                return createAccount.Errors;
            }
            catch (Exception ex)
            {
                await createShopTransaction.RollbackAsync(cancellationToken);

                return new Error(ErrorCode.Exception, ex.Message);
            }
        }
    }
}
