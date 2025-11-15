using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Invoice.DataAccess.DataAccessBase.RepositoryDp
{
    public class Repository<T> : IRepository<T> where T : class
    {
       private readonly InvoiceDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository()
        {
            _context = new InvoiceDbContext();
            _dbSet = _context.Set<T>();    
        }

        public bool Add(T item)
        {
            _dbSet.Add(item);
           return _context.SaveChanges() > 0 ? true : false;
        }

        public bool Delete(T item)
        {
            _dbSet.Remove(item);
            return _context.SaveChanges() > 0 ? true : false;
        }

        public List<T> GetAll(List<T> items)
        {
          return _dbSet.ToList();
        }

        public T GetByPredicate(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.OrderBy(predicate).LastOrDefault();
        }

        public bool Update(T item)
        {
            _dbSet.Update(item);
            return _context.SaveChanges() > 0 ? true : false;
        }
    }
}
