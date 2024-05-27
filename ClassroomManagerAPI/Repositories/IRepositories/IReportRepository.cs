using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
	public interface IReportRepository : IBaseRepository<Report>
	{
		public Task<IEnumerable<Report>> GetByClassroomAddressAsync(string address, int? page, int? limit);
	}
}
