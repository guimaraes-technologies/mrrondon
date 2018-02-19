using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Security.Helpers;

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
                return Ok(_db.Users.Find(AccountManager.UserId));
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
                return Ok(_db.FavoriteEvents.Include(i => i.Event).Where(x => x.UserId == AccountManager.UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("event/favorite")]
        public IHttpActionResult MarkAsFavorite([FromBody]FavoriteEvent model)
        {
            try
            {
                _db.FavoriteEvents.Add(model);
                _db.SaveChanges();

                return Ok(model);
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
                _db.FavoriteEvents.Remove(model);
                _db.SaveChanges();

                return Ok(true);
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