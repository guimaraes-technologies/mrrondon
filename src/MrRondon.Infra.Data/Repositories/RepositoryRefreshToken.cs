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

        public async Task<bool> AddAsync(RefreshToken token)
        {
            var existingToken = _dbSet.FirstOrDefault(r => r.Subject == token.Subject && r.ApplicationClientId == token.ApplicationClientId);

            if (existingToken != null) return await RemoveAsync(existingToken);

            _dbSet.Add(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(RefreshToken entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null) return false;

            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}