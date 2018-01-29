using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.Authorization;

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
                return Ok(_db.FavoriteEvents.Include(i => i.Event).Where(x => x.UserId == Authentication.Current.UserId).Select(s => s.Event));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}