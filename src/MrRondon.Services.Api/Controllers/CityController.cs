using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Services.Api.Context;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/city")]
    public class CityController : ApiController
    {
        private readonly MainContext _db;

        public CityController()
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
                return Ok(_db.Cities.Where(x => x.Name.Contains(name)));
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