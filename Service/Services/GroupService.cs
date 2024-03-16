using Domain.Models;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Constants;
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

            Group group = _groupRepository.GetById(data.Id) ?? throw new NotFoundException(ResponseMessages.DataNotFound);

            if (!string.IsNullOrEmpty(data.Name))
            {
                group.Name = data.Name;
            }

            if (!string.IsNullOrEmpty(data.Teacher))
            {
                group.Teacher = data.Teacher;
            }

            if (!string.IsNullOrEmpty(data.Room))
            {
                group.Room = data.Room;
            }

            _groupRepository.Update(group);
        }

        public void Delete(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);

            Group group = _groupRepository.GetById((int)id) ?? throw new NotFoundException(ResponseMessages.DataNotFound);

            _groupRepository.Delete(group);
        }

        public List<Group> GetAll()
        {
            return _groupRepository.GetAll();
        }

        public List<Group> GetAllWithExpression(Func<Group, bool> predicate)
        {
            return _groupRepository.GetAllWithExpression(predicate).ToList();
        }

        public Group GetById(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return _groupRepository.GetById((int)id) ?? throw new NotFoundException(ResponseMessages.DataNotFound);
        }
    }
}
