using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Helpers;
using ClassroomManagerAPI.Models.Excels;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
    public class UploadScheduleCommand : IRequest<ResponseMethod<Stream>>
    {
        public IFormFile FormFile { get; set; }
    }

    public class UploadScheduleCommandHandler : IRequestHandler<UploadScheduleCommand, ResponseMethod<Stream>>
    {
        private readonly IMediator _mediator;
        private readonly IAuthRepository _authRepostitory;
        private readonly IClassroomRepository _classroomRepository;

        public UploadScheduleCommandHandler(IMediator mediator, IAuthRepository authRepository, IClassroomRepository classroomRepository)
        {
            _mediator = mediator;
            _authRepostitory = authRepository;
            _classroomRepository = classroomRepository;
        }
        public async Task<ResponseMethod<Stream>> Handle(UploadScheduleCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var responseMethod = new ResponseMethod<Stream>();
            if (request.FormFile == null)
            {
                responseMethod.AddBadRequest(nameof(ErrorSystemEnum.ImportFileRequired));
                return responseMethod;
            }
            var result = request.FormFile.ImportAndValidateExcel(async (ImportScheduleModel x, IList<ImportScheduleModel> models, int rowIndex, IList<ValidateExcelModel> errors) =>
            {
                if (string.IsNullOrEmpty(x.Subject))
                {
                    errors.Add(new ValidateExcelModel { RowIndex = rowIndex, ColumnName = nameof(x.Subject), Message = "Subject is null" });
                }
                if (string.IsNullOrEmpty(x.Class))
                {
                    errors.Add(new ValidateExcelModel { RowIndex = rowIndex, ColumnName = nameof(x.Class), Message = "Class is null" });
                }
                return await Task.FromResult(errors.Count == 0);
            });

            if (result.Stream != null)
            {
                responseMethod.Data = result.Stream;
                responseMethod.StatusCode = StatusCodes.Status200OK;
                return responseMethod;
            }
            try
            {
                foreach(var schedule in result.Datas.ToList())
                {
                    string[] dates = schedule.WeekStudy.Split("-");
                    DateTime DateStart = DateTime.Parse(dates[0]);
                    DateTime DateEnd = DateTime.Parse(dates[1]);
                    var classId = await _classroomRepository.Queryable().FirstOrDefaultAsync(x => x.Address.ToLower().Trim().Equals(schedule.Class.ToLower().Trim()),cancellationToken);
                    var accountId = await _authRepostitory.Queryable().FirstOrDefaultAsync(x => x.Email.ToLower().Trim().Equals(schedule.Email.ToLower().Trim()));
                    var Start = DateTime.Parse(schedule.DateStart);
                    while(Start.Date < DateStart.Date || Start.Date > DateEnd.Date)
                    {
                        var startTime = Start.Date.AddDateTime(schedule.StartTime);
                        var endTime = Start.Date.AddDateTime(schedule.EndTime);
                        await _mediator.Send(new AddScheduleCommand
                        {
                            AccountId = accountId.Id,
                            ClassroomId = classId.Id,
                            Subject = schedule.Subject,
                            CountStudent = schedule.CountStudent,
                            StartTime = startTime,
                            EndTime = endTime
                        });
                    }
                }
            }catch(Exception ex)
            {
                responseMethod.AddBadRequest(ex.Message);
            }
            return responseMethod;
        }
    }
}
