
using Domain.Models;

namespace Repository.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        List<Group> GetAllByTeacher(string teacher);
        List<Group> GetAllByRoom(string room);
    }
}
