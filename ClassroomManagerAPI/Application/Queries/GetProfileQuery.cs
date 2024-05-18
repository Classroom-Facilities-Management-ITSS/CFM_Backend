using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries
{
	public class GetProfileQuery : IRequest<Response<UserModel>>
    {
    }

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Response<UserModel>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AuthContext _authContext;

        public GetProfileQueryHandler(AppDbContext dbContext, IMapper mapper, AuthContext authContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authContext = authContext;
        }
        public async Task<Response<UserModel>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            Response<UserModel> result = new Response<UserModel>();
            var id = _authContext.GetCurrentId();
            if( id == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }

            var account = await _dbContext.Users.Include(x => x.Account).ToListAsync(cancellationToken);

            var user = account.FirstOrDefault(u => u.AccountId.ToString() == id);

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
