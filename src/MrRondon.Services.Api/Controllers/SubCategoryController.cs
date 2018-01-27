using System;
using System.Linq;
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
        [Route("{name:alpha=}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name ?? string.Empty;
                return Ok(_db.Categories.Where(x => x.SubCategoryId != null && x.Name.Contains(name)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("{categoryId:int}/{name:alpha=}")]
        public IHttpActionResult GetByCategory(int categoryId, string name)
        {
            try
            {
                name = name ?? string.Empty;
                return Ok(_db.Categories.Where(x => x.SubCategoryId == categoryId && x.Name.Contains(name)));
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