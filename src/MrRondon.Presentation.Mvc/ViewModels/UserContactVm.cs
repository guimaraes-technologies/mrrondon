using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class UsuarioContatoVm
    {
        [Key]
        public Guid UsuarioId { get; set; }

        [DisplayName("Primeiro Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string Nome { get; set; }
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string Sobrenome { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [Remote("CpfIsInUse", "User","Admin", ErrorMessage = "Esse CPF está em uso.")]
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
        public int? CompanyId { get; set; }
    }
}