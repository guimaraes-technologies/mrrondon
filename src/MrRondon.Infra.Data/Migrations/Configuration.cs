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
            var userIds = new[]
            {
                Guid.Parse("2A3B3A45-2C1C-4CE1-9618-9D5AA6A2D56F"),
                Guid.Parse("1C868C4A-9EBD-4C8A-91FC-C326A4E9CAE1")
            };
            if (!context.Users.Any())
            {
                var emails = new[] { "administrator", "user" };

                var roles = new List<Role>
                {
                    new Role {RoleId = 1, Name = "Admin", Description = "Usuário que controla o sistema."},
                };

                for (var i = 0; i < emails.Length; i++)
                {
                    var cpf = i + 1;
                    var user = new User
                    {
                        UserId = userIds[i],
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

            if (!context.ApplicationClients.Any())
            {
                context.ApplicationClients.Add(new ApplicationClient
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

            if (!context.SubCategories.Any())
            {
                context.SubCategories.AddRange(new List<SubCategory>
                {
                    new SubCategory
                    {
                        SubCategoryId = 1,
                        Name = "Aventura"
                    },
                    new SubCategory
                    {
                        SubCategoryId = 2,
                        Name = "Rapel",
                        CategoryId = 1
                    },
                    new SubCategory
                    {
                        SubCategoryId = 3,
                        Name = "Camping's",
                        CategoryId = 1
                    },
                    new SubCategory
                    {
                        SubCategoryId = 4,
                        Name = "Aeroclube",
                        CategoryId = 1
                    },
                    new SubCategory
                    {
                        SubCategoryId = 5,
                        Name = "Kart",
                        CategoryId = 1
                    },
                    new SubCategory
                    {
                        SubCategoryId = 6,
                        Name = "Paintball",
                        CategoryId = 1
                    },
                    new SubCategory
                    {
                        SubCategoryId = 7,
                        Name = "AirSoft",
                        CategoryId = 1
                    },
                    new SubCategory
                    {
                        SubCategoryId = 8,
                        Name = "Trilha",
                        CategoryId = 1
                    },



                    new SubCategory
                    {
                        SubCategoryId = 9,
                        Name = "Hospedagem"
                    },
                    new SubCategory
                    {
                        SubCategoryId = 10,
                        Name = "Hotéis",
                        CategoryId = 9
                    },
                    new SubCategory
                    {
                        SubCategoryId = 11,
                        Name = "Hotéis Fazenda",
                        CategoryId = 9
                    },
                    new SubCategory
                    {
                        SubCategoryId = 12,
                        Name = "Pousadas",
                        CategoryId = 9
                    },
                    new SubCategory
                    {
                        SubCategoryId = 13,
                        Name = "Resort's",
                        CategoryId = 9
                    },

                    new SubCategory
                    {
                        SubCategoryId = 14,
                        Name = "Gastronomia"
                    },
                    new SubCategory
                    {
                        SubCategoryId = 15,
                        Name = "Restaurantes",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 16,
                        Name = "Pizzarias",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 17,
                        Name = "Lanchonetes",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 18,
                        Name = "Restaurantes",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 19,
                        Name = "Food Truck",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 20,
                        Name = "Food Truck",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 21,
                        Name = "Cafeterias",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 22,
                        Name = "Sorveterias",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 23,
                        Name = "Açaiterias",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 24,
                        Name = "Bares",
                        CategoryId = 14
                    },
                    new SubCategory
                    {
                        SubCategoryId = 25,
                        Name = "Drive Thru",
                        CategoryId = 14
                    },


                    new SubCategory
                    {
                        SubCategoryId = 26,
                        Name = "Rent a Car"
                    },


                    new SubCategory
                    {
                        SubCategoryId = 27,
                        Name = "Entretenimento",
                    },
                    new SubCategory
                    {
                        SubCategoryId = 28,
                        Name = "Botecos",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 29,
                        Name = "Boates",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 30,
                        Name = "Bilhares",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 31,
                        Name = "Shopping",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 32,
                        Name = "Cinemas",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 33,
                        Name = "Treatros",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 34,
                        Name = "Casas de Espetáculos",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 35,
                        Name = "Boliches",
                        CategoryId = 27
                    },
                    new SubCategory
                    {
                        SubCategoryId = 36,
                        Name = "Praças",
                        CategoryId = 27
                    },


                    new SubCategory
                    {
                        SubCategoryId = 37,
                        Name = "Parques"
                    },
                    new SubCategory
                    {
                        SubCategoryId = 38,
                        Name = "Parques Aquáticos",
                        CategoryId = 37
                    },
                    new SubCategory
                    {
                        SubCategoryId = 39,
                        Name = "Parques Temáticos",
                        CategoryId = 37
                    },
                    new SubCategory
                    {
                        SubCategoryId = 40,
                        Name = "Parques Naturais",
                        CategoryId = 37
                    },


                    new SubCategory
                    {
                        SubCategoryId = 41,
                        Name = "Marinas"
                    },


                    new SubCategory
                    {
                        SubCategoryId = 42,
                        Name = "Serviços Úteis"
                    },
                    new SubCategory
                    {
                        SubCategoryId = 43,
                        Name = "Aeroporto",
                        CategoryId = 42
                    },
                    new SubCategory
                    {
                        SubCategoryId = 44,
                        Name = "Rodoviária",
                        CategoryId = 42
                    },
                    new SubCategory
                    {
                        SubCategoryId = 45,
                        Name = "Coopetaxi",
                        CategoryId = 42
                    },
                    new SubCategory
                    {
                        SubCategoryId = 46,
                        Name = "Hospitais",
                        CategoryId = 42
                    },
                    new SubCategory
                    {
                        SubCategoryId = 47,
                        Name = "Polícia",
                        CategoryId = 42
                    },


                    new SubCategory
                    {
                        SubCategoryId = 48,
                        Name = "Locação de Equipamentos"
                    },


                    new SubCategory
                    {
                        SubCategoryId = 49,
                        Name = "Centro de Convenções"
                    },


                    new SubCategory
                    {
                        SubCategoryId = 50,
                        Name = "Transporte Turístico"
                    },


                    new SubCategory
                    {
                        SubCategoryId = 51,
                        Name = "Agências de Turismo"
                    },


                    new SubCategory
                    {
                        SubCategoryId = 52,
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
                    ZipCode = "76.817-003",
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
                            SubCategoryId = 5
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

            if (!context.HistoricalSights.Any())
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
                var historicalSight = new HistoricalSight
                {
                    HistoricalSightId = 1,
                    AddressId = address1.AddressId,
                    Address = address1,
                    Name = "Principe da Beira",
                    SightHistory = "A história é interessante, mas outro vai contar  . . . "
                };
                context.HistoricalSights.Add(historicalSight);
            }

            if (!context.Messages.Any())
            {
                context.Messages.AddRange(new List<Message>
                {
                    new Message
                    {
                        MessageId = Guid.NewGuid(),
                        Title = "Não x lida",
                        Description = "Nada a declarar",
                        CellPhone = "(69) 99226-6791",
                        Telephone = "(69) 3211-6791",
                        Status = MessageStatus.Attended,
                        Subject = MessageSubject.NewCompany,
                        UserId = userIds[1]
                    },
                    new Message
                    {
                        MessageId = Guid.NewGuid(),
                        Title = "Não  lida x x",
                        Description = "Nada a declarar",
                        CellPhone = "(69) 99226-6791",
                        Telephone = "(69) 3211-6791",
                        Status = MessageStatus.Read,
                        Subject = MessageSubject.UpdateCompany,
                        UserId = userIds[1]
                    },
                    new Message
                    {
                        MessageId = Guid.NewGuid(),
                        Title = "Lida agora",
                        Description = "Nada a declarar",
                        CellPhone = "(69) 99226-6791",
                        Telephone = "(69) 3211-6791",
                        Status = MessageStatus.Read,
                        Subject = MessageSubject.UpdateEvent,
                        UserId = userIds[0]
                    },
                    new Message
                    {
                        MessageId = Guid.NewGuid(),
                        Title = "Não  lida",
                        Description = "Nada 645 64 a xx",
                        CellPhone = "(69) 99226-6791",
                        Telephone = "(69) 3211-6791",
                        Status = MessageStatus.Unread,
                        Subject = MessageSubject.UpdateCompany,
                        UserId = userIds[0]
                    },
                    new Message
                    {
                        MessageId = Guid.NewGuid(),
                        Title = "Não  lida",
                        Description = "Nada a declarar",
                        CellPhone = "(69) 99226-6791",
                        Telephone = "(69) 3211-6791",
                        Status = MessageStatus.Read,
                        Subject = MessageSubject.NewEvent,
                        UserId = userIds[1]
                    },
                    new Message
                    {
                        MessageId = Guid.NewGuid(),
                        Title = "Lida read",
                        Description = "Nada a declarar read",
                        CellPhone = "(69) 99226-6791",
                        Telephone = "(69) 3211-6791",
                        Status = MessageStatus.Attended,
                        Subject = MessageSubject.NewEvent,
                        UserId = userIds[0]
                    }
                });
            }
            context.SaveChanges();
        }
    }
}