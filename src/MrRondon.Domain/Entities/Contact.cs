using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Contact
    {
        [Key]
        public Guid ContactId { get; set; }
        public string Description { get; set; }
        public ContactType ContactType { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }

        public void Atualizar(Contact item)
        {
            Description = item.Description;
            ContactType = item.ContactType;
        }
    }
}