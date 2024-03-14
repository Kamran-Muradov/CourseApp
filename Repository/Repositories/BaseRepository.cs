using Domain.Common;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        public void Create(T entity)
        {
            AppDbContext<T>.datas.Add(entity);
        }

        public void Update(T entity)
        {
            AppDbContext<T>.datas[entity.Id] = entity;
        }

        public void Delete(T entity)
        {
            AppDbContext<T>.datas.Remove(entity);
        }
        
        public List<T> GetAll()
        {
            return AppDbContext<T>.datas.ToList();
        }

        public List<T> GetAllWithExpression(Func<T, bool> predicate)
        {
            return AppDbContext<T>.datas.Where(predicate).ToList();
        }

        public T GetById(int id)
        {
            return AppDbContext<T>.datas.FirstOrDefault(m => m.Id == id);
        }
    }
}
