using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/company")]
    public class CompanyController : ApiController
    {
        private readonly MainContext _db;

        public CompanyController()
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
                return Ok(_db.Companies.Where(x => x.Name.Contains(name) || x.FancyName.Contains(name)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("{name:alpha=}")]
        public IHttpActionResult Get(int segmentId, string name)
        {
            try
            {
                name = name ?? string.Empty;
                return Ok(_db.Companies.Where(x => x.SegmentId == segmentId && (x.Name.Contains(name) || x.FancyName.Contains(name))));
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