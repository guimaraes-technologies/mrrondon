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
        [Route("{eventId:guid}")]
        public IHttpActionResult Get(Guid eventId)
        {
            try
            {
                var item = _db.Events
                    .Include(i => i.Address.City)
                    .Include(i => i.Contacts)
                    .Include(i => i.Organizer)
                    .FirstOrDefault(f => f.EventId == eventId);

                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("nearby/meters/{precision:int}/latitude/{latitudeFrom}/longitude/{longitudeFrom}")]
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

                var result = items.Select(s =>
                    new
                    {
                        s.EventId,
                        s.Name,
                        s.Logo,
                        s.Address
                    });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("city/{cityId:int}/{name=}")]
        public IHttpActionResult Get(int cityId, string name)
        {
            try
            {
                name = string.IsNullOrWhiteSpace(name) ? string.Empty : name;
                var events = _db.Events.Include(i => i.Address.City).Where(x => x.Address.CityId == cityId && x.Name.Contains(name));
                var result = events.Select(s =>
                    new
                    {
                        s.EventId,
                        s.Name,
                        s.Logo,
                        s.StartDate,
                        s.EndDate,
                        s.Value
                    });

                return Ok(result);
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