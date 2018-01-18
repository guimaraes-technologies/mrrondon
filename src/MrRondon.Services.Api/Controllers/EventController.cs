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

        [Route("{city}")]
        public IHttpActionResult Get(string city)
        {
            return Ok(_db.Events);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}