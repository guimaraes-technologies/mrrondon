﻿using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/historicalsight")]
    public class HistoricalSightController : ApiController
    {
        private readonly MainContext _db;

        public HistoricalSightController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var item = _db.HistoricalSights
                    .Include(i => i.Address.City)
                    .FirstOrDefault(f => f.HistoricalSightId == id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("{name=}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name ?? string.Empty;
                return Ok(_db.HistoricalSights.Where(x => x.Name.Contains(name)));
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