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
    public class UpdateFacilityCommand : UpdateFacilityModel ,IRequest<ResponseMethod<string>>
    {
    }

    public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, ResponseMethod<string>>
    {
        private readonly IFacilityRepository _facilityRepository;
		private readonly IClassroomRepository _classroomRepository;
		private readonly IMapper _mapper;
        public UpdateFacilityCommandHandler(IMapper mapper, IFacilityRepository facilityRepository, IClassroomRepository classroomRepository)
        {
            _facilityRepository = facilityRepository;
			_classroomRepository = classroomRepository;
			_mapper = mapper;
        }
        public async Task<ResponseMethod<string>> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<string> result = new ResponseMethod<string>();

            var classroomExsiting = await _classroomRepository.GetByIDAsync(request.ClassroomId).ConfigureAwait(false);
            if (classroomExsiting == null)
            {
                result.AddBadRequest(nameof(ErrorClassEnum.ClassroomNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }

            if(await _facilityRepository.Queryable.AnyAsync(x => !x.IsDeleted && x.Name.ToLower().Trim().Equals(request.Name.ToLower().Trim())).ConfigureAwait(false))
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
                result.StatusCode = (int)HttpStatusCode.Conflict;
                return result;
            }

            var facility = _mapper.Map<Entities.Facility>(request);
            var existingFacility = await _facilityRepository.GetByIDAsync(request.Id);
            var updatedFacility = await _facilityRepository.UpdateAsync(facility).ConfigureAwait(false);
            if (updatedFacility == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;  
            }
            classroomExsiting.FacilityAmount += request.Count - existingFacility.Count;
            await _classroomRepository.UpdateAsync(classroomExsiting).ConfigureAwait(false);
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = $"update facility with id {request.Id} sucessfully";
            return result;
        }
    }
}
