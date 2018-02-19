using MrRondon.Domain.Entities;

namespace MrRondon.Services.Api.ViewModels
{
    public class CategoryListVm
    {
        public CategoryListVm(SubCategory category, bool hasSubCategory)
        {
            SubCategoryId = category.SubCategoryId;
            Name = category.Name;
            Image = category.Image;
            HasSubCategory = hasSubCategory;
        }

        public int SubCategoryId { get; private set; }
        public string Name { get; private set; }
        public byte[] Image { get; private set; }
        public bool HasSubCategory { get; private set; }
    }
}