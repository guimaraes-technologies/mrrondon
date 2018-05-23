using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.ViewModels;
using WebApi.OutputCache.V2;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/category")]
    [CacheOutput(ClientTimeSpan = 50, ServerTimeSpan = 50)]
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
                var categories = _db.SubCategories.Where(w => w.CategoryId == null && w.ShowOnApp && w.Name.Contains(name) && (w.Companies.Any() || w.SubCategories.Any(a => a.Companies.Any()))).AsNoTracking();

                var items = categories
                .Select(s => new CategoryListVm
                {
                    SubCategoryId = s.SubCategoryId,
                    Name = s.Name,
                    Image = s.Image,
                    HasCompany = s.Companies.Any(),
                    HasSubCategory = s.SubCategories.Any()
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