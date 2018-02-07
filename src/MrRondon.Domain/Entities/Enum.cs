using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public enum ContactType
    {
        Telephone = 1,
        Cellphone = 2,
        Email = 3
    }
    
    public enum MessageSubject
    {
        [Display(Name = "Cadastro de empresa")]
        NewCompany = 1,
        [Display(Name = "Atualização de empresa")]
        UpdateCompany = 2,
        [Display(Name = "Cadastro de evento")]
        NewEvent = 3,
        [Display(Name = "Atualização de evento")]
        UpdateEvent = 4
    }

    public enum MessageStatus
    {
        [Display(Name = "Não Lida")]
        Unread = 1,
        [Display(Name = "Lida")]
        Read = 2,
        [Display(Name = "Atendida")]
        Attended = 3
    }
}