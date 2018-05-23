using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;

namespace MrRondon.Infra.Data.Repositories
{
    public class RepositoryRefreshToken
    {
        private readonly MainContext _context;
        private readonly DbSet<RefreshToken> _dbSet;

        public RepositoryRefreshToken(MainContext context)
        {
            _context = context;
            _dbSet = _context.Set<RefreshToken>();
        }

        public RefreshToken Find(string id)
        {
            return _dbSet.Find(id);
        }

        public bool Add(RefreshToken token)
        {
            var existingToken = _dbSet.FirstOrDefault(r => r.Subject == token.Subject && r.ApplicationClientId == token.ApplicationClientId);

            if (existingToken != null) return Remove(existingToken);

            _dbSet.Add(token);

            return _context.SaveChanges() > 0;
        }

        public bool Remove(RefreshToken entity)
        {
            _dbSet.Remove(entity);
            return _context.SaveChanges() > 0;
        }
    }
}