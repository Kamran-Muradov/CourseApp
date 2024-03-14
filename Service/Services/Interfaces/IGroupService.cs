using Domain.Models;

namespace Service.Services.Interfaces
{
    public interface IGroupService
    {
        void Create(Group data);
        void Update(Group data);
        void Delete(int? id);
        List<Group> GetAll();
        Group GetById(int? id);
        List<Group> GetAllByTeacher(string teacher);
        List<Group> GetAllByRoom(string room);
        List<Group> SearchByName(string searchText);
    }
}
