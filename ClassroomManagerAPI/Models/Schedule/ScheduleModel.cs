
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Classroom;

namespace ClassroomManagerAPI.Models.Schedule
{
	public class ScheduleModel : AddScheduleModel
	{
        public Guid Id { get; set; }
        public ClassroomModel? Classroom { get; set; }
        public AccountModel? Account { get; set; }
    }
}
