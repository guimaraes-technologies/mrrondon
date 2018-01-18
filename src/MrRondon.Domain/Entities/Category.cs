using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public int? SubCategoryId { get; set; }
        public Category SubCategory { get; set; }
    }
}