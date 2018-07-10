using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class UserContactVm
    {
        [Key]
        public Guid UserId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }

        [DisplayName("Sobrenome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string LastName { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [Remote("CpfIsInUse", "User", "Admin", ErrorMessage = "Esse CPF está em uso.")]
        public string Cpf { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.Now;
        public int AccessFailed { get; set; }
        public ICollection<Role> Roles { get; set; }
        public List<Contact> Contacts { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Tipo de contato")]
        public ContactType ContactType { get; set; }

        [Display(Name = "Permissões")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public int[] RolesIds { get; set; } = { };

        public IList<SelectListItemVm> SelectListRole { get; set; } = new List<SelectListItemVm>();

        public User GetUser()
        {
            var user = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Contacts = Contacts,
                AccessFailed = AccessFailed,
                Cpf = Cpf,
                IsActive = IsActive,
                CreateOn = CreateOn,
                UserId = UserId,
                Roles = new List<Role>(RolesIds.Select(s => new Role { RoleId = s }))
            };

            return user;
        }

        public bool IsValid()
        {
            if (!RolesIds?.Any() ?? true) throw new Exception("O campo Permissões é obrigatório.");
            if (!Contacts?.Any(contact => contact.ContactType == ContactType.Email) ?? true) throw new Exception("Pelo um Email de contato deve ser informado.");

            return true;
        }
    }
}