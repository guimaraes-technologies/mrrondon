using System;
using System.Linq;
using System.Collections.Generic;
using MrRondon.Domain.Entities;

namespace MrRondon.Infra.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Context.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context.MainContext context)
        {
            if (!context.Users.Any())
            {
                var emails = new[] { "administrator", "user" };
                var roles = new List<Role>
                {
                    new Role {RoleId = 1, Name = "Admin", Description = "Usuário que controla o sistema."},
                };

                var users = new List<User>();
                for (var i = 0; i < emails.Length; i++)
                {
                    var cpf = i + 1;
                    var user = new User
                    {
                        UserId = Guid.NewGuid(),
                        FirstName = "User",
                        LastName = "Master",
                        Cpf = string.Format("{0}{0}{0}.{0}{0}{0}.{0}{0}{0}-{0}{0}", cpf),
                        AccessFailed = 0,
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                ContactId = Guid.NewGuid(),
                                Description = $"{emails[i]}@gmail.com",
                                ContactType = ContactType.Email
                            }
                        },
                        Roles = new List<Role> { roles[0] }
                    };
                    user.EncryptPassword("111111");
                    context.Users.Add(user);
                }
            }

            var city1 = new City
            {
                CityId = 1,
                Name = "Porto Velho"
            };
            var city2 = new City
            {
                CityId = 2,
                Name = "Ouro Preto D'Oeste"
            };

            if (!context.Cities.Any())
            {
                context.Cities.Add(city1);
                context.Cities.Add(city2);
            }

            if (!context.Clients.Any())
            {
                context.Clients.Add(new ApplicationClient
                {
                    ApplicationClientId = Guid.NewGuid(),
                    Secret = "Mr.Rondon.Turismo.App",
                    Name = "mrrondon.app",
                    RefreshTokenLifeTime = 1,
                    ApplicationType = ApplicationTypes.NativeConfidential,
                    AllowedOrigin = "*",
                    Active = true

                });
            }

            if (!context.Categories.Any())
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
                        Name = "Rapel",
                        SubCategoryId = 1
                    },
                    new Category
                    {
                        CategoryId = 3,
                        Name = "Camping's",
                        SubCategoryId = 1
                    },
                    new Category
                    {
                        CategoryId = 4,
                        Name = "Aeroclube",
                        SubCategoryId = 1
                    },
                    new Category
                    {
                        CategoryId = 5,
                        Name = "Kart",
                        SubCategoryId = 1
                    },
                    new Category
                    {
                        CategoryId = 6,
                        Name = "Paintball",
                        SubCategoryId = 1
                    },
                    new Category
                    {
                        CategoryId = 7,
                        Name = "AirSoft",
                        SubCategoryId = 1
                    },
                    new Category
                    {
                        CategoryId = 8,
                        Name = "Trilha",
                        SubCategoryId = 1
                    },



                    new Category
                    {
                        CategoryId = 9,
                        Name = "Hospedagem"
                    },
                    new Category
                    {
                        CategoryId = 10,
                        Name = "Hotéis",
                        SubCategoryId = 9
                    },
                    new Category
                    {
                        CategoryId = 11,
                        Name = "Hotéis Fazenda",
                        SubCategoryId = 9
                    },
                    new Category
                    {
                        CategoryId = 12,
                        Name = "Pousadas",
                        SubCategoryId = 9
                    },
                    new Category
                    {
                        CategoryId = 13,
                        Name = "Resort's",
                        SubCategoryId = 9
                    },

                    new Category
                    {
                        CategoryId = 14,
                        Name = "Gastronomia"
                    },
                    new Category
                    {
                        CategoryId = 15,
                        Name = "Restaurantes",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 16,
                        Name = "Pizzarias",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 17,
                        Name = "Lanchonetes",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 18,
                        Name = "Restaurantes",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 19,
                        Name = "Food Truck",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 20,
                        Name = "Food Truck",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 21,
                        Name = "Cafeterias",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 22,
                        Name = "Sorveterias",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 23,
                        Name = "Açaiterias",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 24,
                        Name = "Bares",
                        SubCategoryId = 14
                    },
                    new Category
                    {
                        CategoryId = 25,
                        Name = "Drive Thru",
                        SubCategoryId = 14
                    },


                    new Category
                    {
                        CategoryId = 26,
                        Name = "Rent a Car"
                    },


                    new Category
                    {
                        CategoryId = 27,
                        Name = "Entretenimento",
                    },
                    new Category
                    {
                        CategoryId = 28,
                        Name = "Botecos",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 29,
                        Name = "Boates",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 30,
                        Name = "Bilhares",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 31,
                        Name = "Shopping",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 32,
                        Name = "Cinemas",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 33,
                        Name = "Treatros",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 34,
                        Name = "Casas de Espetáculos",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 35,
                        Name = "Boliches",
                        SubCategoryId = 27
                    },
                    new Category
                    {
                        CategoryId = 36,
                        Name = "Praças",
                        SubCategoryId = 27
                    },


                    new Category
                    {
                        CategoryId = 37,
                        Name = "Parques"
                    },
                    new Category
                    {
                        CategoryId = 38,
                        Name = "Parques Aquáticos",
                        SubCategoryId = 37
                    },
                    new Category
                    {
                        CategoryId = 39,
                        Name = "Parques Temáticos",
                        SubCategoryId = 37
                    },
                    new Category
                    {
                        CategoryId = 40,
                        Name = "Parques Naturais",
                        SubCategoryId = 37
                    },


                    new Category
                    {
                        CategoryId = 41,
                        Name = "Marinhas"
                    },


                    new Category
                    {
                        CategoryId = 42,
                        Name = "Serviços Úteis"
                    },
                    new Category
                    {
                        CategoryId = 43,
                        Name = "Aeroporto",
                        SubCategoryId = 42
                    },
                    new Category
                    {
                        CategoryId = 44,
                        Name = "Rodoviária",
                        SubCategoryId = 42
                    },
                    new Category
                    {
                        CategoryId = 45,
                        Name = "Coopetaxi",
                        SubCategoryId = 42
                    },
                    new Category
                    {
                        CategoryId = 46,
                        Name = "Hospitais",
                        SubCategoryId = 42
                    },
                    new Category
                    {
                        CategoryId = 47,
                        Name = "Polícia",
                        SubCategoryId = 42
                    },


                    new Category
                    {
                        CategoryId = 48,
                        Name = "Locação de Equipamentos"
                    },


                    new Category
                    {
                        CategoryId = 49,
                        Name = "Centro de Convenções"
                    },


                    new Category
                    {
                        CategoryId = 50,
                        Name = "Transporte Turístico"
                    },


                    new Category
                    {
                        CategoryId = 51,
                        Name = "Agências de Turismo"
                    },


                    new Category
                    {
                        CategoryId = 52,
                        Name = "Promotores de Eventos"
                    }
                });
            }

            if (!context.Companies.Any())
            {
                var address1 = new Address
                {
                    AddressId = Guid.NewGuid(),
                    CityId = city1.CityId,
                    City = city1,
                    ZipCode = "11111-111",
                    Number = "1234",
                    Neighborhood = "Bairro Novo",
                    Street = "Rodovia BR 364, km 702",
                    Latitude = -8.799778,
                    Longitude = -63.807484
                };

                context.Companies.AddRange(
                    new List<Company>
                    {
                        new Company
                        {
                            CompanyId = Guid.NewGuid(),
                            AddressId = address1.AddressId,
                            Address = address1,
                            Name = "Guimaraes Tecnologia",
                            FancyName = "Guimares Tecnologia LTDA",
                            Cnpj = "04.956.000/001-00",
                            SegmentId = 5
                        }
                    });
            }

            if (!context.Events.Any())
            {
                var address1 = new Address
                {
                    AddressId = Guid.NewGuid(),
                    Latitude = -8.7526757,
                    Longitude = -63.9128231,
                    Neighborhood = "Centro",
                    Number = "S/N",
                    Street = "Farquar",
                    ZipCode = "76.817-003",
                    CityId = city1.CityId,
                    City = city1
                };

                var address2 = new Address
                {
                    AddressId = Guid.NewGuid(),
                    Latitude = -8.751807,
                    Longitude = -63.910008,
                    Neighborhood = "Centro",
                    Number = "S/N",
                    Street = "Farquar",
                    ZipCode = "76.817-003",
                    CityId = city2.CityId,
                    City = city2
                };

                var address3 = new Address
                {
                    AddressId = Guid.NewGuid(),
                    Latitude = -8.799778,
                    Longitude = -63.807484,
                    Neighborhood = "Cidade Jardim",
                    Number = "S/N",
                    Street = "BR 364, KM 702",
                    ZipCode = "76.817-003",
                    CityId = city2.CityId,
                    City = city2
                };

                var address4 = new Address
                {
                    AddressId = Guid.NewGuid(),
                    Latitude = -8.804051,
                    Longitude = -63.803288,
                    Neighborhood = "Bairro Novo",
                    Number = "S/N",
                    Street = "Condomínio Residencial Amarilis",
                    ZipCode = "76.817-003",
                    CityId = city2.CityId,
                    City = city2
                };

                context.Events.AddRange(
                    new List<Event>
                    {
                        new Event
                        {
                            EventId = Guid.NewGuid(),
                            AddressId = address1.AddressId,
                            Address = address1,
                            Name = "Luminato Festival",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(2),
                            Value = 10
                        },
                        new Event
                        {
                            EventId = Guid.NewGuid(),
                            AddressId = address2.AddressId,
                            Address = address2,
                            Name = "Canadian National Exhibition",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(1),
                            Value = 100
                        },
                        new Event
                        {
                            EventId = Guid.NewGuid(),
                            AddressId = address3.AddressId,
                            Address = address3,
                            Name = "Fim de semana em família",
                            StartDate = DateTime.Now.AddDays(3),
                            EndDate = DateTime.Now.AddDays(3),
                            Value = 150
                        },
                        new Event
                        {
                            EventId = Guid.NewGuid(),
                            AddressId = address4.AddressId,
                            Address = address4,
                            Name = "Reunião do condomínio",
                            StartDate = DateTime.Now.AddDays(4),
                            EndDate = DateTime.Now.AddDays(5),
                            Value = 1
                        }
                    }
                );

            }

            context.SaveChanges();
        }
    }
}