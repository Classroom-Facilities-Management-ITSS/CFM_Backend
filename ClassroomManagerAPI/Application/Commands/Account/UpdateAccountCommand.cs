using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Account
{
    public class UpdateAccountCommand : UpdateAccountModel, IRequest<ResponseMethod<string>>
    {
    }

    public class UpdateAccountCommmandHandler : IRequestHandler<UpdateAccountCommand, ResponseMethod<string>>
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public UpdateAccountCommmandHandler(IMapper mapper, IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
        }
        public async Task<ResponseMethod<string>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<string> result = new ResponseMethod<string>();
            var account = _mapper.Map<Entities.Account>(request);
            var existingAccount = await _accountRepository.Queryable
                .Where(a => a.Email == request.Email && !a.IsDeleted && a.Id != request.Id)
                .FirstOrDefaultAsync();
            if (existingAccount != null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
                result.StatusCode = (int)HttpStatusCode.Conflict;
                return result;
            }
            var updatedAccount = await _accountRepository.UpdateAsync(account).ConfigureAwait(false);
            if (updatedAccount == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = $"update Account with id {request.Id} sucessfully";
            return result;
        }
    }
}