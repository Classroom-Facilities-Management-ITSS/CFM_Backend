using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Helpers;
using ClassroomManagerAPI.Models.Excels;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
    public class UploadScheduleCommand : IRequest<ResponseMethod<Stream>>
    {
        public IFormFile? FormFile { get; set; }
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
                string format = "dd/MM/yyyy";
                foreach (var schedule in result.Datas.ToList())
                {
                    string[] dates = schedule.WeekStudy.Split("-");
                    DateTime DateStart = DateTime.ParseExact(dates[0].Trim(), format, CultureInfo.InvariantCulture);
                    DateTime DateEnd = DateTime.ParseExact(dates[1].Trim(), format, CultureInfo.InvariantCulture);
                    var classId = await _classroomRepository.Queryable().FirstOrDefaultAsync(x => x.Address.ToLower().Trim().Equals(schedule.Class.ToLower().Trim()),cancellationToken);
                    var accountId = await _authRepostitory.Queryable().FirstOrDefaultAsync(x => x.Email.ToLower().Trim().Equals(schedule.Email.ToLower().Trim()));
                    var Start = DateTime.Parse(schedule.DateStart);
                    while(Start.Date >= DateStart.Date && Start.Date <= DateEnd.Date)
                    {
                        var startTime = Start.Date.AddDateTime(schedule.StartTime);
                        var endTime = Start.Date.AddDateTime(schedule.EndTime);
                        var newSchedule = await _mediator.Send(new AddScheduleCommand
                        {
                            AccountId = accountId?.Id,
                            ClassroomId = classId?.Id,
                            Subject = schedule.Subject,
                            CountStudent = Int32.Parse(schedule.CountStudent),
                            StartTime = startTime,
                            EndTime = endTime
                        }).ConfigureAwait(false);
                        Start = Start.Date.AddDays(7);
                    }
                }
                responseMethod.StatusCode = StatusCodes.Status204NoContent;
            }catch(Exception ex)
            {
                responseMethod.AddBadRequest(ex.Message);
            }
            return responseMethod;
        }
    }
}
