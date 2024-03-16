using Domain.Models;

namespace Repository.Repositories.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        //List<Student> GetAllByAge(int age);
        //List<Student> GetAllByGroupId(int groupId);
    }
}
