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
	public class UpdateFacilityCommand : UpdateFacilityModel, IRequest<ResponseMethod<FacilityModel>>
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

			// Validate the new classroom
			var newClassroom = await _classroomRepository.GetByIDAsync(request.ClassroomId).ConfigureAwait(false);
			if (newClassroom == null)
			{
				result.AddBadRequest(nameof(ErrorClassEnum.ClassroomNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}

			// Get the existing facility
			var existingFacility = await _facilityRepository.GetByIDAsync(request.Id);
			if (existingFacility == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}

			// Check if the facility name already exists in the new classroom
			var facilityExist = await _facilityRepository.Queryable
				.AnyAsync(x => !x.IsDeleted && x.ClassroomId == request.ClassroomId && x.Id != request.Id && x.Name.ToLower().Trim().Equals(request.Name.ToLower().Trim()))
				.ConfigureAwait(false);
			if (facilityExist)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
				result.StatusCode = (int)HttpStatusCode.Conflict;
				return result;
			}

			// Adjust facility amounts for classrooms if the ClassroomId is changed
			if (existingFacility.ClassroomId != request.ClassroomId)
			{
				// Decrease FacilityAmount of the old classroom
				var oldClassroom = await _classroomRepository.GetByIDAsync(existingFacility.ClassroomId).ConfigureAwait(false);
				if (oldClassroom != null)
				{
					oldClassroom.FacilityAmount -= existingFacility.Count;
					await _classroomRepository.UpdateAsync(oldClassroom).ConfigureAwait(false);
				}

				// Increase FacilityAmount of the new classroom
				newClassroom.FacilityAmount += request.Count;
			}
			else
			{
				// Adjust FacilityAmount for the same classroom
				newClassroom.FacilityAmount += request.Count - existingFacility.Count;
			}

			await _classroomRepository.UpdateAsync(newClassroom).ConfigureAwait(false);

			// Update the facility
			var facility = _mapper.Map<Entities.Facility>(request);
			var updatedFacility = await _facilityRepository.UpdateAsync(facility).ConfigureAwait(false);

			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = _mapper.Map<FacilityModel>(updatedFacility);
			return result;
		}
	}
}
