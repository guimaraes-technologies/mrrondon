using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Security.Entities;
using Person = MrRondon.Domain.Entities.Person;

namespace MrRondon.Services.Api.Context
{
    public class MainContext : DbContext
    {
        public MainContext() : base("MainContext") { }

        public DbSet<Address> Adresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<FavoriteEvent> FavoriteEvents { get; set; }
        public DbSet<HistoricalSight> HistoricalSights { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}