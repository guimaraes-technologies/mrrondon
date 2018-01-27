using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.Helpers;

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
        [Route("nearby/meters/{precision:int}/latitude/{latitudeFrom:double}/longitude/{longitudeFrom:double}")]
        public IHttpActionResult GetNearby(int precision, string latitudeFrom, string longitudeFrom)
        {
            try
            {
                var latitude = double.Parse(latitudeFrom);
                var longitude = double.Parse(longitudeFrom);
                var events = _db.Events.Include(i => i.Address.City).ToList();

                var items = (from item in events
                           where GeoLocatorHelper.PlacesAround(latitude, longitude, item.Address.Latitude, item.Address.Longitude, precision) <= precision
                           select item);

                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("{city:alpha=}")]
        public IHttpActionResult Get(string city)
        {
            try
            {
                city = city ?? string.Empty;
                return Ok(_db.Events.Include(i => i.Address.City).Where(x => x.Address.City.Name.Contains(city)));
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