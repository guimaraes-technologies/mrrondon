namespace MrRondon.Services.Api.ViewModels
{
    public class CategoryListVm
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get;  set; }
        public bool HasCompany{ get; set; }
        public bool HasSubCategory { get; set; }
    }
}