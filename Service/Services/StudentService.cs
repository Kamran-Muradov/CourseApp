using Domain.Models;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private int count = 1;

        public StudentService()
        {
            _studentRepository = new StudentRepository();
        }
        public void Create(Student data)
        {
            ArgumentNullException.ThrowIfNull(data);

            data.Id = count++;
            _studentRepository.Create(data);
        }

        public void Update(Student data)
        {
            ArgumentNullException.ThrowIfNull(data);

            Student student = _studentRepository.GetById(data.Id) ?? throw new NotFoundException(ResponseMessages.DataNotFound);

            _studentRepository.Update(student);
        }

        public void Delete(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);

            Student student = _studentRepository.GetById((int)id);

            if (student is null) throw new NotFoundException(ResponseMessages.DataNotFound);

            _studentRepository.Delete(student);
        }

        public List<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public Student GetById(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return _studentRepository.GetById((int)id);
        }

        public List<Student> GetAllByAge(int? age)
        {
            ArgumentNullException.ThrowIfNull(age);

            return _studentRepository.GetAllByAge((int)age);
        }

        public List<Student> GetAllByGroupId(int? groupId)
        {
            ArgumentNullException.ThrowIfNull(groupId);

            return _studentRepository.GetAllByGroupId((int)groupId);
        }

        public List<Student> SearchByNameOrSurname(string searchText)
        {
            ArgumentNullException.ThrowIfNull(searchText);

            return _studentRepository.GetAllWithExpression(m => m.Name.Trim().ToLower().Contains(searchText) || m.Surname.Trim().ToLower().Contains(searchText));
        }
    }
}
