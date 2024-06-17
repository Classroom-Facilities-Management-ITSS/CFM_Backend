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
    public class UpdateFacilityCommand : UpdateFacilityModel ,IRequest<ResponseMethod<FacilityModel>>
    {
    }

    public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, ResponseMethod<FacilityModel>>
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
        public async Task<ResponseMethod<FacilityModel>> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<FacilityModel>();

            var classroomExsiting = await _classroomRepository.GetByIDAsync(request.ClassroomId).ConfigureAwait(false);
            if (classroomExsiting == null)
            {
                result.AddBadRequest(nameof(ErrorClassEnum.ClassroomNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            var existingFacility = await _facilityRepository.GetByIDAsync(request.Id);
            if (existingFacility == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            var facilityExist = await _facilityRepository.Queryable.AnyAsync(x => !x.IsDeleted && x.ClassroomId == request.ClassroomId && x.Id != request.Id && x.Name.ToLower().Trim().Equals(request.Name.ToLower().Trim())).ConfigureAwait(false);
            if (facilityExist)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
                result.StatusCode = (int)HttpStatusCode.Conflict;
                return result;
            }

            if(classroomExsiting.Id == existingFacility.ClassroomId)
            {
                classroomExsiting.FacilityAmount += request.Count - existingFacility.Count;
            }
            else
            {
                var CurrentClass = await _classroomRepository.GetByIDAsync(existingFacility.ClassroomId).ConfigureAwait(false);
                CurrentClass.FacilityAmount -= existingFacility.Count;
                await _classroomRepository.UpdateAsync(CurrentClass).ConfigureAwait(false);
                classroomExsiting.FacilityAmount += request.Count;
            }
            var newClass = await _classroomRepository.UpdateAsync(classroomExsiting).ConfigureAwait(false);
            _mapper.Map(request, existingFacility);
            var updatedFacility = await _facilityRepository.UpdateAsync(existingFacility).ConfigureAwait(false);            
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = _mapper.Map<FacilityModel>(updatedFacility);
            return result;
        }
    }
}
