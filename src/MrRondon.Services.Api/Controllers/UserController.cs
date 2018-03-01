using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.Authorization;
using Newtonsoft.Json.Linq;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/user")]
    public class UserController : ApiController
    {
        private readonly MainContext _db;

        public UserController()
        {
            _db = new MainContext();
        }

        [Route("information")]
        public IHttpActionResult GetInformation()
        {
            try
            {
                return Ok(_db.Users.Find(Authentication.Current.UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("event/favorites")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_db.FavoriteEvents.Include(i => i.Event).Where(x => x.UserId == Authentication.Current.UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("event/{eventId:guid}/favorite")]
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

        [HttpPost]
        [Route("event/unfavorite")]
        public IHttpActionResult UnMarkAsFavorite([FromBody]FavoriteEvent model)
        {
            try
            {
                if (_db.FavoriteEvents.Any(a => a.EventId == model.EventId)) return Ok(false);
                var favoriteEvent = _db.FavoriteEvents.FirstOrDefault(f =>
                    f.EventId == model.EventId && f.UserId == Authentication.Current.UserId);

                if (favoriteEvent == null) return Ok(false);

                _db.FavoriteEvents.Remove(favoriteEvent);
                _db.SaveChanges();

                return Ok(false);
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