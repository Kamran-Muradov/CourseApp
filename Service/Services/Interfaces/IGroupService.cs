using Domain.Models;
using System;

namespace Service.Services.Interfaces
{
    public interface IGroupService
    {
        void Create(Group data);
        void Update(Group data);
        void Delete(int? id);
        List<Group> GetAll();
        List<Group> GetAllWithExpression(Func<Group, bool> predicate);
        Group GetById(int? id);
    }
}
