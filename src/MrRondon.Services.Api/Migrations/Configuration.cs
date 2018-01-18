using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using MrRondon.Domain.Entities;

namespace MrRondon.Services.Api.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MrRondon.Services.Api.Context.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context.MainContext context)
        {
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