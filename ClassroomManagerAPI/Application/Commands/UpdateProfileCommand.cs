using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.User;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Application.Commands
{
    public class UpdateProfileCommand : UpdateUserModel , IRequest<ResponseMethod<UserModel>>
    {
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateProfileCommand, ResponseMethod<UserModel>>
    {
        private readonly IMapper _mapper;
        private readonly AuthContext _authContext;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IMapper mapper, AuthContext authContext, IUserRepository userRepository)
        {
            _mapper = mapper;
            _authContext = authContext;
            _userRepository = userRepository;
        }
        public async Task<ResponseMethod<UserModel>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<UserModel>();
            if(request.AccountId == null)
            {
                request.AccountId = _authContext.GetCurrentId();
                if (request.AccountId == null)
                {
                    result.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
                    result.StatusCode = StatusCodes.Status404NotFound;
                    return result;
                }
            }
            var infor = await _userRepository.Queryable().FirstOrDefaultAsync(x =>!x.IsDeleted && x.AccountId == request.AccountId, cancellationToken);
            if(infor == null)
            {
                result.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
                result.StatusCode = StatusCodes.Status404NotFound;
                return result;
            }
            var newInfor = _mapper.Map<UserInfo>(request);
            newInfor.Id = infor.Id;
            var updateInfor = await _userRepository.UpdateAsync(newInfor).ConfigureAwait(false);
            result.StatusCode = StatusCodes.Status200OK;
            result.Data = _mapper.Map<UserModel>(updateInfor);
            return result;
        }
    }
}
