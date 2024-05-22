using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Account
{
	public class UpdateAccountCommand : UpdateAccountModel, IRequest<ResponseMethod<string>>
	{
	}

	public class UpdateAccountCommmandHandler : IRequestHandler<UpdateAccountCommand, ResponseMethod<string>>
	{
		private readonly IMapper mapper;
		private readonly IAccountRepository accountRepository;

		public UpdateAccountCommmandHandler(IMapper mapper, IAccountRepository accountRepository)
        {
			this.mapper = mapper;
			this.accountRepository = accountRepository;
		}
        public async Task<ResponseMethod<string>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var facility = mapper.Map<Entities.Account>(request);
			var updatedFacility = await accountRepository.UpdateAsync(facility).ConfigureAwait(false);
			if (updatedFacility == null)
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
