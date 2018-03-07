using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class UserContactVm
    {
        [Key]
        public Guid UserId { get; set; }

        [DisplayName("Primeiro Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string LastName { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [Remote("CpfIsInUse", "User", "Admin", ErrorMessage = "Esse CPF está em uso.")]
        public string Cpf { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.Now;
        public ICollection<Role> Roles { get; set; }
        public List<Contact> Contacts { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Tipo de contato")]
        public ContactType ContactType { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public int[] RolesIds { get; set; }
        public Guid? CompanyId { get; set; }

        public User GetUser()
        {
            var user = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Contacts = Contacts,
                AccessFailed = 0,
                Cpf = Cpf,
                IsActive = true
            };

            return user;
        }
    }
}