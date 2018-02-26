using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.ViewModels;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/category")]
    public class CategoryController : ApiController
    {
        private readonly MainContext _db;

        public CategoryController()
        {
            _db = new MainContext();
        }

        [Route("{question}")]
        public IHttpActionResult Test(string question)
        {
            return Ok("Logou");
        }

        [AllowAnonymous]
        [Route("{name=}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name ?? string.Empty;
                var categories = _db.SubCategories.Where(w => w.CategoryId == null && w.ShowOnApp && w.Name.Contains(name)).ToList();

                var categoriesWithSubCategoriesHasCompany = _db.SubCategories.Where(w => w.Categories.Any(x => x.Companies.Any())).ToList();

                categoriesWithSubCategoriesHasCompany.AddRange(categories);

                    var items = categoriesWithSubCategoriesHasCompany
                    .Select(s => new CategoryListVm
                    {
                        SubCategoryId = s.SubCategoryId,
                        Name = s.Name,
                        Image = s.Image,
                        HasCompany = s.Companies.Any(),
                        HasSubCategory = s.Categories.Any()
                    }).Distinct().ToList();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}