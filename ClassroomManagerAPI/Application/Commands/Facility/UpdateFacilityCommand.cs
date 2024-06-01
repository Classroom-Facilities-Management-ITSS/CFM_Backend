using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
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
            var facility = _mapper.Map<Entities.Facility>(request);
            var existingFacility = await _facilityRepository.GetByIDAsync(request.Id);
            if ( existingFacility == null )
            {
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
            _mapper.Map(request, existingFacility);
            if (existingFacility.ClassroomId != request.ClassroomId)
            {
				if (existingFacility.ClassroomId.HasValue)
				{
					var oldClassroom = await _classroomRepository.GetByIDAsync(existingFacility.ClassroomId.Value);
					if (oldClassroom != null && oldClassroom.Facilities != null)
					{
						oldClassroom.Facilities.Remove(existingFacility);
						await _classroomRepository.UpdateAsync(oldClassroom).ConfigureAwait(false);
					}
				}
				if (request.ClassroomId != null)
				{
					var newClassroom = await _classroomRepository.GetByIDAsync(request.ClassroomId);
					if (newClassroom != null)
					{
						if (newClassroom.Facilities == null)
						{
							newClassroom.Facilities = new List<ClassroomManagerAPI.Entities.Facility>();
						}
						newClassroom.Facilities.Add(existingFacility);
						await _classroomRepository.UpdateAsync(newClassroom).ConfigureAwait(false);
					}
				}
			}
            var updatedFacility = await _facilityRepository.UpdateAsync(facility).ConfigureAwait(false);
            if (updatedFacility == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;  
            }
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = $"update facility with id {request.Id} sucessfully";
            return result;
        }
    }
}
