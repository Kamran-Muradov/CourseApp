using Domain.Models;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private int count = 1;

        public GroupService()
        {
            _groupRepository = new GroupRepository();
        }
        public void Create(Group data)
        {
            ArgumentNullException.ThrowIfNull(data);

            data.Id = count++;
            _groupRepository.Create(data);
        }

        public void Update(Group data)
        {
            ArgumentNullException.ThrowIfNull(data);

            _groupRepository.Update(data);
        }

        public void Delete(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);

            Group group = _groupRepository.GetById((int)id);

            if (group is null) throw new NotFoundException("Data notfound");

            _groupRepository.Delete(group);
        }

        public List<Group> GetAll()
        {
            return _groupRepository.GetAll();
        }

        public Group GetById(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return _groupRepository.GetById((int)id);
        }

        public List<Group> SearchByName(string searchText)
        {
            ArgumentNullException.ThrowIfNull(searchText);

            return _groupRepository.GetAllWithExpression(m => m.Name.Contains(searchText));
        }

        public List<Group> GetAllByTeacher(string teacher)
        {
            ArgumentNullException.ThrowIfNull(teacher);

            return _groupRepository.GetAllByTeacher(teacher);
        }

        public List<Group> GetAllByRoom(string room)
        {
            ArgumentNullException.ThrowIfNull(room);

            return _groupRepository.GetAllByRoom(room);
        }
    }
}
