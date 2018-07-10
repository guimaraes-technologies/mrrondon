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
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(Context.MainContext context)
        {
            var userIds = new[]
            {
                Guid.Parse("1C868C4A-9EBD-4C8A-91FC-C326A4E9CAE1"),
                Guid.Parse("2C868C4A-9EBD-4C8A-91FC-C326A4E9CAE2"),
                Guid.Parse("3C868C4A-9EBD-4C8A-91FC-C326A4E9CAE3"),
                Guid.Parse("4C868C4A-9EBD-4C8A-91FC-C326A4E9CAE4"),
                Guid.Parse("5C868C4A-9EBD-4C8A-91FC-C326A4E9CAE5"),
                Guid.Parse("6C868C4A-9EBD-4C8A-91FC-C326A4E9CAE6"),
                Guid.Parse("7C868C4A-9EBD-4C8A-91FC-C326A4E9CAE7"),
                Guid.Parse("8C868C4A-9EBD-4C8A-91FC-C326A4E9CAE8"),
                Guid.Parse("9C868C4A-9EBD-4C8A-91FC-C326A4E9CAE9"),
                Guid.Parse("10868C4A-9EBD-4C8A-91FC-C326A4E9CA10")
            };
            var roles = new List<Role>
            {
                new Role
                {
                    RoleId = 1,
                    Name = "Administrador_Geral",
                    Description = "Usuário que controla todo o sistema."
                },
                new Role
                {
                    RoleId = 2,
                    Name = "Administrador_Usuário",
                    Description = "Usuário que controla os usuários."
                },
                new Role
                {
                    RoleId = 3,
                    Name = "Administrador_Categoria",
                    Description = "Usuário que controla as categorias."
                },
                new Role
                {
                    RoleId = 4,
                    Name = "Administrador_Cidade",
                    Description = "Usuário que controla as cidades."
                },
                new Role
                {
                    RoleId = 5,
                    Name = "Administrador_Empresa",
                    Description = "Usuário que controla as empresas."
                },
                new Role
                {
                    RoleId = 6,
                    Name = "Administrador_Evento",
                    Description = "Usuário que controla os eventos."
                },
                new Role
                {
                    RoleId = 7,
                    Name = "Administrador_Empresa",
                    Description = "Usuário que controla as empresas."
                },
                new Role
                {
                    RoleId = 8,
                    Name = "Administrador_Memorial",
                    Description = "Usuário que controla os memoriais."
                },
                new Role
                {
                    RoleId = 9,
                    Name = "Administrador_Sub_Categoria",
                    Description = "Usuário que controla as sub-categorias."
                },
                new Role
                {
                    RoleId = 10,
                    Name = "Consulta",
                    Description = "Usuário que apenas irá visualizar as informações."
                }
            };

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(roles);
            }

            if (!context.Users.Any())
            {
                for (var i = 0; i < roles.Count; i++)
                {
                    var cpf = i + 1;
                    var user = new User
                    {
                        UserId = userIds[i],
                        FirstName = "User",
                        LastName = roles[i].Name.Replace("Administrador_", ""),
                        Cpf = string.Format("{0}{0}{0}.{0}{1}{0}.{1}{0}{0}-{1}{0}", cpf.ToString().Substring(0, 1), Random.Next(9)),
                        AccessFailed = 0,
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                ContactId = Guid.NewGuid(),
                                Description = $"{RandomString(4 + i)}@gmail.com",
                                ContactType = ContactType.Email,
                                UserId = userIds[i]
                            }
                        },
                        Roles = new List<Role> { roles[i] }
                    };
                    user.SetNewPassword("111111");
                    context.Users.Add(user);
                }
            }


            if (!context.ApplicationClients.Any())
            {
                //context.ApplicationClients.Add(new ApplicationClient
                //{
                //    ApplicationClientId = Guid.NewGuid(),
                //    Secret = "Rondonia.Turismo.App",
                //    Name = "Aplicativo Rond�nia Turismo",
                //    RefreshTokenLifeTime = 1,
                //    ApplicationType = ApplicationTypes.NativeConfidential,
                //    AllowedOrigin = "*",
                //    Active = true
                //
                //});
            }

            //if (!context.SubCategories.Any())
            //{
            //    context.SubCategories.AddRange(new List<SubCategory>
            //    {
            //        new SubCategory
            //        {
            //            SubCategoryId = 1,
            //            Name = "Aventura"
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 2,
            //            Name = "Rapel",
            //            CategoryId = 1
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 3,
            //            Name = "Camping's",
            //            CategoryId = 1
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 4,
            //            Name = "Aeroclube",
            //            CategoryId = 1
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 5,
            //            Name = "Kart",
            //            CategoryId = 1
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 6,
            //            Name = "Paintball",
            //            CategoryId = 1
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 7,
            //            Name = "AirSoft",
            //            CategoryId = 1
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 8,
            //            Name = "Trilha",
            //            CategoryId = 1
            //        },



            //        new SubCategory
            //        {
            //            SubCategoryId = 9,
            //            Name = "Hospedagem"
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 10,
            //            Name = "Hot�is",
            //            CategoryId = 9
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 11,
            //            Name = "Hot�is Fazenda",
            //            CategoryId = 9
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 12,
            //            Name = "Pousadas",
            //            CategoryId = 9
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 13,
            //            Name = "Resort's",
            //            CategoryId = 9
            //        },

            //        new SubCategory
            //        {
            //            SubCategoryId = 14,
            //            Name = "Gastronomia"
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 15,
            //            Name = "Restaurantes",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 16,
            //            Name = "Pizzarias",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 17,
            //            Name = "Lanchonetes",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 18,
            //            Name = "Restaurantes",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 19,
            //            Name = "Food Truck",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 20,
            //            Name = "Food Truck",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 21,
            //            Name = "Cafeterias",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 22,
            //            Name = "Sorveterias",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 23,
            //            Name = "A�aiterias",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 24,
            //            Name = "Bares",
            //            CategoryId = 14
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 25,
            //            Name = "Drive Thru",
            //            CategoryId = 14
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 26,
            //            Name = "Rent a Car"
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 27,
            //            Name = "Entretenimento",
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 28,
            //            Name = "Botecos",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 29,
            //            Name = "Boates",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 30,
            //            Name = "Bilhares",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 31,
            //            Name = "Shopping",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 32,
            //            Name = "Cinemas",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 33,
            //            Name = "Treatros",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 34,
            //            Name = "Casas de Espet�culos",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 35,
            //            Name = "Boliches",
            //            CategoryId = 27
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 36,
            //            Name = "Pra�as",
            //            CategoryId = 27
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 37,
            //            Name = "Parques"
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 38,
            //            Name = "Parques Aqu�ticos",
            //            CategoryId = 37
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 39,
            //            Name = "Parques Tem�ticos",
            //            CategoryId = 37
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 40,
            //            Name = "Parques Naturais",
            //            CategoryId = 37
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 41,
            //            Name = "Marinas"
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 42,
            //            Name = "Servi�os �teis"
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 43,
            //            Name = "Aeroporto",
            //            CategoryId = 42
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 44,
            //            Name = "Rodovi�ria",
            //            CategoryId = 42
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 45,
            //            Name = "Coopetaxi",
            //            CategoryId = 42
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 46,
            //            Name = "Hospitais",
            //            CategoryId = 42
            //        },
            //        new SubCategory
            //        {
            //            SubCategoryId = 47,
            //            Name = "Pol�cia",
            //            CategoryId = 42
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 48,
            //            Name = "Loca��o de Equipamentos"
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 49,
            //            Name = "Centro de Conven��es"
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 50,
            //            Name = "Transporte Tur�stico"
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 51,
            //            Name = "Ag�ncias de Turismo"
            //        },


            //        new SubCategory
            //        {
            //            SubCategoryId = 52,
            //            Name = "Promotores de Eventos"
            //        }
            //    });
            //}

            //if (!context.HistoricalSights.Any())
            //{
            //    var address1 = new Address
            //    {
            //        AddressId = Guid.NewGuid(),
            //        Latitude = -8.7526757,
            //        Longitude = -63.9128231,
            //        Neighborhood = "Centro",
            //        Number = "S/N",
            //        Street = "Farquar",
            //        ZipCode = "76.817-003",
            //        CityId = 1
            //    };
            //    var historicalSight = new HistoricalSight
            //    {
            //        HistoricalSightId = 1,
            //        AddressId = address1.AddressId,
            //        Address = address1,
            //        Name = "Principe da Beira",
            //        SightHistory = "A hist�ria � interessante, mas outro vai contar  . . . "
            //    };
            //    context.HistoricalSights.Add(historicalSight);
            //}

            context.SaveChanges();
        }

        private static readonly Random Random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}