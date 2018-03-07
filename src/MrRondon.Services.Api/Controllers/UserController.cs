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
        
        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}