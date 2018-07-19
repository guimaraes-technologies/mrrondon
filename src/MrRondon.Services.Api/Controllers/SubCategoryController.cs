using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using WebApi.OutputCache.V2;

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
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 120)]
        public IHttpActionResult GetByCategory(int categoryId, string name)
        {
            try
            {
                name = name ?? string.Empty;
                var items = _db.SubCategories
                    .Where(x => x.ShowOnApp && x.Companies.Any(a => a.SubCategoryId == x.SubCategoryId) && x.CategoryId == categoryId && x.Name.Contains(name))
                    .Select(s => new
                    {
                        s.SubCategoryId,
                        s.Name
                    });

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