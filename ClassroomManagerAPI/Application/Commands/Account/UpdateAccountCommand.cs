using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
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
        private readonly IBCryptService _bcryptService;

        public UpdateAccountCommmandHandler(IMapper mapper, IAccountRepository accountRepository, IBCryptService bCryptService)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _bcryptService = bCryptService;
        }
        public async Task<ResponseMethod<string>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<string> result = new ResponseMethod<string>();
            var existingAccount = await _accountRepository.Queryable
                .Where(a => a.Email == request.Email && !a.IsDeleted && a.Id != request.Id)
                .FirstOrDefaultAsync();
            if (existingAccount != null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
                result.StatusCode = (int)HttpStatusCode.Conflict;
                return result;
            }
            var account = await _accountRepository.GetByIDAsync(request.Id.Value).ConfigureAwait(false);
            request.Password = _bcryptService.HashPassword(request.Password);
            _mapper.Map(request, account);
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