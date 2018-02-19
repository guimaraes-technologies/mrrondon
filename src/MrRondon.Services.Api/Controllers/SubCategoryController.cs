using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using MrRondon.Infra.Data.Context;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/subcategory")]
    public class SubCategoryController : ApiController
    {
        private readonly MainContext _db;

        public SubCategoryController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        [Route("{categoryId:int}/{name:alpha=}")]
        public IHttpActionResult GetByCategory(int categoryId, string name)
        {
            try
            {
                name = name ?? string.Empty;
                var subCategories = _db.SubCategories
                    .Include(i => i.Category)
                    .Where(x =>
                    x.Companies.Any(a => a.SubCategoryId == x.SubCategoryId) && x.ShowOnApp &&
                    x.CategoryId == categoryId && x.Name.Contains(name));

                return Ok(subCategories.Select(s => new
                {
                    s.SubCategoryId,
                    s.Name
                }));
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