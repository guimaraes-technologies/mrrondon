using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;

namespace MrRondon.Infra.Data.Repositories
{
    public class RepositoryRefreshToken
    {
        protected MainContext Context;
        protected DbSet<RefreshToken> DbSet;

        public async Task<RefreshToken> FindAsync(string id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<bool> AddAsync(RefreshToken token)
        {
            var existingToken = DbSet.FirstOrDefault(r => r.Subject == token.Subject && r.ApplicationClientId == token.ApplicationClientId);

            if (existingToken != null) return await RemoveAsync(existingToken);

            DbSet.Add(token);

            return Context.SaveChanges() > 0;
        }

        public async Task<bool> RemoveAsync(RefreshToken entity)
        {
            DbSet.Remove(entity);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(object id)
        {
            var entity = await DbSet.FindAsync(id);

            if (entity == null) return false;

            DbSet.Remove(entity);
            return await Context.SaveChangesAsync() > 0;
        }
    }
}