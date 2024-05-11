using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
	public interface IClassroomRepository : IBaseRepository<Classroom?>
	{
		public Task<Classroom?> GetByAddressAsync(string address);
	}
}
