using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading.Tasks;
using MrRondon.Domain.Entities;

namespace MrRondon.Infra.Data.Context
{
    public class MainContext : DbContext
    {
        public MainContext() : base("MainContext")
        {
            Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Address> Adresses { get; set; }
        public DbSet<ApplicationClient> ApplicationClients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<FavoriteEvent> FavoriteEvents { get; set; }
        public DbSet<HistoricalSight> HistoricalSights { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Configurações gerais do Contexto
            modelBuilder.Properties().Where(p => p.Name == p.ReflectedType.Name + "Id").Configure(p => p.IsKey());
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(100));

            modelBuilder.Entity<Category>()
                .HasOptional(p => p.SubCategory)
                .WithMany()
                .HasForeignKey(p => p.SubCategoryId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(u => u.Users)
                .Map(m =>
                {
                    m.ToTable("UserRole");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.Entries)
                {
                    sb.Append($"ERROS:\n{failure.State}\n{failure.Entity.GetType().Name}");
                }
                var erro = sb.ToString();
                throw;
            }
            catch (DbUnexpectedValidationException un)
            {
                var erro = un.Message;
                throw;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                return await SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.Entries)
                {
                    sb.Append($"ERROS:\n{failure.State}\n{failure.Entity.GetType().Name}");
                }
                var erro = sb.ToString();
                throw;
            }
            catch (DbUnexpectedValidationException un)
            {
                var erro = un.Message;
                throw;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                throw;
            }
        }
    }
}