using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
	public interface IClassroomRepository : IBaseRepository<Classroom?>
	{
		public Task<IEnumerable<Classroom>?> GetByAddressAsync(string address, int? page, int? limit);
	}
}
