using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;

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

        [AllowAnonymous]
        [Route("{name:alpha=}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name ?? string.Empty;
                return Ok(_db.SubCategories.Where(x => x.ShowOnApp && x.CategoryId == null && x.Name.Contains(name)));
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