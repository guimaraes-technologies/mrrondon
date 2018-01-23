using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using MrRondon.Services.Api.Context;
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

                var poiipu = _db.Events
                    .Include(i => i.Address.City)
                    .Where(x => GeoLocatorHelper.PlacesAround(latitude, longitude, x.Address.Latitude, x.Address.Longitude, precision) <= precision);


                var t = _db.EventsNearby(latitude, longitude, precision);
                    

                var list = (from it in _db.Events.Include(i => i.Address.City)
                            where GeoLocatorHelper.PlacesAround(latitude, longitude, it.Address.Latitude, it.Address.Longitude, precision) <= precision
                            select it);

                var sss = (from item in events
                           where GeoLocatorHelper.PlacesAround(latitude, longitude, item.Address.Latitude, item.Address.Longitude, precision) <= precision
                           select item);

                return Ok(sss);
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

    public static class Teste
    {
        public static object EventsNearby(this MainContext ctx, double latitude, double longitude, int precision)
        {
            ctx.Events.;
        }

        
    }
}