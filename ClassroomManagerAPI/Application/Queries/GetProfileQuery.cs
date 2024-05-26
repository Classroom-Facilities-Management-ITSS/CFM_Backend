using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.User;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries
{
	public class GetProfileQuery : IRequest<ResponseMethod<UserModel>>
    {
    }

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ResponseMethod<UserModel>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly AuthContext _authContext;

        public GetProfileQueryHandler(IAccountRepository accountRepository, IMapper mapper, AuthContext authContext)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _authContext = authContext;
        }
        public async Task<ResponseMethod<UserModel>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<UserModel> result = new ResponseMethod<UserModel>();
            var id = _authContext.GetCurrentId();
            if( id == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }

            var user = await _accountRepository.Queryable().Include(x => x.UserInfo).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if(user == null )
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }

            result.Data = _mapper.Map<UserModel>(user);
            result.StatusCode = (int)HttpStatusCode.OK;
            return result;
        }
    }
}
