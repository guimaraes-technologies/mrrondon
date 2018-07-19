namespace MrRondon.Infra.CrossCutting.Helper
{
    public class RegularExpressions
    {
        public const string Cpf = @"^\d{3}\.\d{3}\.\d{3}-\d{2}$";
        public const string Cnpj = @"^\d{3}\x2E\d{3}\x2E\d{3}\x2D\d{2}$";
        public const string Telephone = @"^\(\d{2}\)\d{4}-\d{4}$";
        public const string Cep = @"^\d{5}-\d{3}$";
        public const string Date = @"^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$";
        public const string Email = @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$";
        public const string LetterAndNumber = @"^([a-zA-Z0-9_.-]*).{6,15}$";
        public const string Numero = @"^\d{1,8}$";
        public const string Cref = @"^(\d{6}\-)([A-Za-z]){1}(\/)([A-Za-z]){2}$";
        public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z]).{6,12}$";
    }
}