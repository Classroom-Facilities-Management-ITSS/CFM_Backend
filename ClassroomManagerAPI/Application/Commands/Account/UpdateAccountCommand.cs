using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Account
{
	public class UpdateAccountCommand : UpdateAccountModel, IRequest<Response<string>>
	{
	}

	public class UpdateAccountCommmandHandler : IRequestHandler<UpdateAccountCommand, Response<string>>
	{
		private readonly IMapper mapper;
		private readonly IAccountRepository accountRepository;

		public UpdateAccountCommmandHandler(IMapper mapper, IAccountRepository accountRepository)
        {
			this.mapper = mapper;
			this.accountRepository = accountRepository;
		}
        public async Task<Response<string>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<string> result = new Response<string>();
			var facility = mapper.Map<Entities.Account>(request);
			var updatedFacility = await accountRepository.UpdateAsync(facility).ConfigureAwait(false);
			if (updatedFacility == null)
			{
				result.AddBadRequest($"Account with id {request.Id} not existing");
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"update Account with id {request.Id} sucessfully";
			return result;
		}
	}
}
