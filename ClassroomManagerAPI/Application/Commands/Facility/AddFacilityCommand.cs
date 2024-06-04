using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Facility
{
    public class AddFacilityCommand : AddFacilityModel, IRequest<ResponseMethod<FacilityModel>>
    {
    }

    public class AddFacilityCommandHandler : IRequestHandler<AddFacilityCommand, ResponseMethod<FacilityModel>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        private readonly IClassroomRepository _classroomRepository;

        public AddFacilityCommandHandler(IMapper mapper, IFacilityRepository facilityRepository, IClassroomRepository classroomRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
            _classroomRepository = classroomRepository;
        }

        public async Task<ResponseMethod<FacilityModel>> Handle(AddFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<FacilityModel> result = new ResponseMethod<FacilityModel>();
            var existingFacility = await _facilityRepository.Queryable
                .Where(a => a.Name.ToLower().Trim().Equals(request.Name.ToLower().Trim()) && a.ClassroomId == request.ClassroomId && !a.IsDeleted)
                .AnyAsync(cancellationToken);
            if (existingFacility)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
                result.StatusCode = (int)HttpStatusCode.Conflict;
                return result;
            }
            var classroom = await _classroomRepository.GetByIDAsync(request.ClassroomId).ConfigureAwait(false);
            if (classroom == null)
            {
				result.AddBadRequest(nameof(ErrorClassEnum.ClassroomNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			var newFacillity = _mapper.Map<Entities.Facility>(request);

            classroom.FacilityAmount += request.Count;
            await _classroomRepository.UpdateAsync(classroom).ConfigureAwait(false);

            var createdFacility = await _facilityRepository.AddAsync(newFacillity).ConfigureAwait(false);
            result.StatusCode = (int) HttpStatusCode.Created;
            result.Data = _mapper.Map<FacilityModel>(createdFacility);
            return result;
        }
    }
}
