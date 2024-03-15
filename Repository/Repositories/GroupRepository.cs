using Domain.Models;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public List<Group> GetAllByTeacher(string teacher)
        {
            return AppDbContext<Group>.datas.Where(m => m.Teacher == teacher).ToList();
        }

        public List<Group> GetAllByRoom(string room)
        {
            return AppDbContext<Group>.datas.Where(m => m.Room == room).ToList();
        }
    }
}
