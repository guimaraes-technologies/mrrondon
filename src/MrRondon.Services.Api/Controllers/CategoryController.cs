using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MrRondon.Infra.Data.Context;
using MrRondon.Services.Api.ViewModels;

namespace MrRondon.Services.Api.Controllers
{
    [RoutePrefix("v1/category")]
    public class CategoryController : ApiController
    {
        private readonly MainContext _db;

        public CategoryController()
        {
            _db = new MainContext();
        }

        [Route("{question}")]
        public IHttpActionResult Test(string question)
        {
            return Ok("Logou");
        }

        [AllowAnonymous]
        [Route("{name=}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name ?? string.Empty;
                var items = (from s in _db.SubCategories
                             join c in _db.Companies on s.SubCategoryId equals c.SubCategoryId
                             where s.ShowOnApp && s.Name.Contains(name)
                             group s by s.SubCategoryId
                    into gp
                             select gp.Select(s => new CategoryListVm
                             {
                                 SubCategoryId = s.SubCategoryId,
                                 Name = s.Name,
                                 Image = s.Image,
                                 HasCompany = s.Companies.Any(),
                                 HasSubCategory = s.Categories.Any()
                             }).FirstOrDefault()).ToList().Distinct();

                return Ok(items);
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