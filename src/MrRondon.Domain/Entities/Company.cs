using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string FancyName { get; set; }
        public string Cnpj { get; set; }

        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        
        public int SubCategoryId { get; set; }
        public Category SubCategory { get; set; }
    }
}