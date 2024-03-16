using Domain.Models;

namespace Service.Services.Interfaces
{
    public interface IStudentService
    {
        void Create(Student data);
        void Update(Student data);
        void Delete(int? id);
        List<Student> GetAll();
        public List<Student> GetAllWithExpression(Func<Student, bool> predicate);
        Student GetById(int? id);
    }
}
