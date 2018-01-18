using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Services.Api.Context;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/event")]
    public class EventController : ApiController
    {
        private readonly MainContext _db;

        public EventController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        [Route("{city=}")]
        public IHttpActionResult Get(string city)
        {
            try
            {
                city = city ?? string.Empty;
                return Ok(_db.Events.Where(x => x.Address.City.Name.Contains(city)));
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