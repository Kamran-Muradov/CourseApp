using Domain.Models;

namespace Service.Services.Interfaces
{
    public interface IStudentService
    {
        void Create(Student data);
        void Update(Student data);
        void Delete(int? id);
        List<Student> GetAll();
        Student GetById(int? id);
        List<Student> GetAllByAge(int? age);
        List<Student> GetAllByGroupId(int? groupId);
        List<Student> SearchByNameOrSurname(string searchText);
    }
}
