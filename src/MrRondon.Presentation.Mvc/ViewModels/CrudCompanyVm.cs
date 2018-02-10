using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class CrudCompanyVm
    {
        public Guid ContactId { get; set; }
        public string Description { get; set; }
        public ContactType ContactType { get; set; }

        [DisplayName("Categoria")]
        public int CategoryId { get; set; }

        [DisplayName("Sub Categoria")]
        public int? SubCategoryId { get; set; }
        public HttpPostedFileBase CoverFile { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }

        public Address Address { get; set; }
        public Company Company { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}