namespace MrRondon.Infra.CrossCutting.Helper
{
    public class Constants
    {
        public const string SystemName = "Rondônia Turismo";

        public static class Roles
        {
            public const string GeneralAdministrator = "Administrador_Geral";
            public const string CategoryAdministrator = "Administrador_Categoria";
            public const string CityAdministrator = "Administrador_Cidade";
            public const string CompanyAdministrator = "Administrador_Empresa";
            public const string EventAdministrator = "Administrador_Evento";
            public const string HistoricalSightAdministrator = "Administrador_Memorial";
            public const string SubCategoryAdministrator = "Administrador_Sub_Categoria";
            public const string UserAdministrator = "Administrador_Usuário";
            public const string ReadOnly = "Consulta";
        }

        public static class Emails
        {
            public const string Setur = "rondonia.setur@gmail.com";
        }

        public static class Codes
        {
            public const int Unauthorized = 401;
            public const int NotFound = 404;
        }
    }
}