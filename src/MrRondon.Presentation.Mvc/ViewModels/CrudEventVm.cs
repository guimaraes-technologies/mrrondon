using System;
using System.Collections.Generic;
using System.Web;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class CrudEventVm
    {
        public Guid ContactId { get; set; }
        public string Description { get; set; }
        public ContactType ContactType { get; set; }

        public Event Event { get; set; }
        public AddressForEventVm Address { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}