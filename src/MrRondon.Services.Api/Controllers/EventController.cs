using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.Authorization;
using MrRondon.Services.Api.Helpers;
using WebApi.OutputCache.V2;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/event")]
    [CacheOutput(ClientTimeSpan = 50, ServerTimeSpan = 50)]
    public class EventController : ApiController
    {
        private readonly MainContext _db;

        public EventController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        [Route("{eventId:guid}")]
        [CacheOutput(ServerTimeSpan = 120)]
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
                item.Logo = null;
                if (Authentication.Current.IsAuthenticated)
                    item.IsFavorite = _db.FavoriteEvents.Any(a => a.EventId == eventId && a.UserId == Authentication.Current.UserId);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{eventId:guid}/isfavorite")]
        public IHttpActionResult IsFavorite(Guid eventId)
        {
            try
            {
                var isfavorite = _db.FavoriteEvents
                    .Any(f => f.EventId == eventId && f.UserId == Authentication.Current.UserId);

                return Ok(isfavorite);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{eventId:guid}/favorite")]
        public IHttpActionResult MarkAsFavorite(Guid eventId)
        {
            try
            {
                var favoriteEvent = _db.FavoriteEvents.FirstOrDefault(f =>
                    f.EventId == eventId && f.UserId == Authentication.Current.UserId);

                if (favoriteEvent == null)
                {
                    favoriteEvent = new FavoriteEvent
                    {
                        EventId = eventId,
                        UserId = Authentication.Current.UserId
                    };

                    _db.FavoriteEvents.Add(favoriteEvent);
                    _db.SaveChanges();
                    return Ok(true);
                }

                _db.FavoriteEvents.Remove(favoriteEvent);
                _db.SaveChanges();

                return Ok(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("favorites")]
        public IHttpActionResult Favorite()
        {
            try
            {
                var favorites = _db.FavoriteEvents
                    .Include(i => i.Event).Where(x => x.UserId == Authentication.Current.UserId);
                foreach (var item in favorites)
                {
                    item.Event.Cover = null;
                }
                return Ok(favorites);
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
                var events = _db.Events
                    .Include(i => i.Address.City)
                    .Where(x => x.StartDate >= DateTime.Today)
                    .AsNoTracking().ToList();

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
        [Route("city/{cityId:int}/{skip:int}/{take:int}/{name=}")]
        public IHttpActionResult Get(int cityId, string name, int skip, int take)
        {
            try
            {
                name = string.IsNullOrWhiteSpace(name) ? string.Empty : name;

                var items = _db.Events
                    .Include(i => i.Address.City)
                    .Where(x => x.Address.CityId == cityId && x.StartDate >= DateTime.Today  && x.Name.Contains(name))
                    .OrderBy(x => x.Name)
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking();
                
                var result = items.Select(s =>
                    new
                    {
                        s.EventId,
                        s.Name,
                        s.Logo,
                        s.StartDate,
                        s.EndDate,
                        s.Value
                    }).ToList();

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