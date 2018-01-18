using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Security.Entities;

namespace MrRondon.Services.Api.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context.MainContext context)
        {
            if (!context.Clients.Any())
            {
                context.Clients.Add(new Client
                {
                    ClientId = Guid.NewGuid(),
                    Secret = "Mr.Rondon.Turismo.App",
                    Name = "mrrondon.app",
                    RefreshTokenLifeTime = 1,
                    ApplicationType = ApplicationTypes.NativeConfidential,
                    AllowedOrigin = "*",
                    Active = true

                });
            }
            if (context.Categories.Any()) return;

            context.Categories.AddRange(new List<Category>
            {
                new Category
                {
                    CategoryId = 1,
                    Name = "Aventura"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Hospedagem"
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "Serviços Úteis"
                },
                new Category
                {
                    CategoryId = 4,
                    Name = "Gastronomia"
                },
                new Category
                {
                    CategoryId = 5,
                    Name = "Aeroclube",
                    SubCategoryId = 1
                },
                new Category
                {
                    CategoryId = 6,
                    Name = "Airsoft",
                    SubCategoryId = 1
                },
                new Category
                {
                    CategoryId = 7,
                    Name = "Camping",
                    SubCategoryId = 1
                }
            });

            context.SaveChanges();
        }
    }
}