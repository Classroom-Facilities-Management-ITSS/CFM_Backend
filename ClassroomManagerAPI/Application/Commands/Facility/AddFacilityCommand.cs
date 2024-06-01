using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
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
        private readonly IMediator _mediator;
        private readonly IClassroomRepository _classroomRepository;

        public AddFacilityCommandHandler(IMapper mapper, IFacilityRepository facilityRepository, IMediator mediator, IClassroomRepository classroomRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
            _mediator = mediator;
            _classroomRepository = classroomRepository;
        }

        public async Task<ResponseMethod<FacilityModel>> Handle(AddFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<FacilityModel> result = new ResponseMethod<FacilityModel>();
            var existingFacility = _facilityRepository.Queryable()
                .Where(a => a.Name == request.Name && a.ClassroomId == request.ClassroomId && !a.IsDeleted)
                .AnyAsync();
            if (existingFacility != null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
                result.StatusCode = (int)HttpStatusCode.Conflict;
                return result;
            }
            var classroom = await _classroomRepository.GetByIDAsync(request.ClassroomId);
            if (classroom == null)
            {
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			var newFacillity = _mapper.Map<Entities.Facility>(request);

            classroom.Facilities.Add(newFacillity);
            classroom.FacilityAmount += request.Count;
            await _classroomRepository.UpdateAsync(classroom).ConfigureAwait(false);

            var createdFacility = await _facilityRepository.AddAsync(newFacillity).ConfigureAwait(false);
            var classroom = _classroomRepository;
            result.StatusCode = (int) HttpStatusCode.Created;
            result.Data = _mapper.Map<FacilityModel>(createdFacility);
            return result;
        }
    }
}
