using System.Linq.Expressions;

namespace Invoice.DataAccess.DataAccessBase.RepositoryDp
{
    public interface IRepository<T> where T : class
    {
        public bool Add(T item);
        public bool Delete(T item);
        public bool Update(T item);
        public T GetByPredicate(Expression<Func<T, bool>> predicate);
        public List<T> GetAll(List<T> items);
    }
}
