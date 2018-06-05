using System.Data.Entity;
using MrRondon.Domain.Interfaces.Repositories;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Migrations;

namespace MrRondon.Infra.Data.Repositories
{
    public class DataBaseManagerRepository : IDataBaseManagerRepository
    {
        public void UpdateToLastedVersion()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MainContext, Configuration>());
        }
    }
}