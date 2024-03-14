using Domain.Models;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public List<Student> GetAllByAge(int age)
        {
            return AppDbContext<Student>.datas.Where(m => m.Age == age).ToList();
        }

        public List<Student> GetAllByGroupId(int groupId)
        {
            return AppDbContext<Student>.datas.Where(m => m.Group.Id == groupId).ToList();

        }
    }
}
