using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Infra.Security.Extensions;
using MrRondon.Presentation.Mvc.Extensions;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    [HasAny("Administrador_Geral")]
    public class MessageController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Unread()
        {
            return View();
        }
        public ActionResult Read()
        {
            return View();
        }
        public ActionResult Attended()
        {
            return View();
        }
        
        public ActionResult Details(Guid id)
        {
            var repo = new RepositoryBase<Message>(_db);
            var model = repo.GetItemByExpression(x => x.MessageId == id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                _db.Messages.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Unread");
            }
            catch (Exception ex)
            {
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Edit(Guid id)
        {
            var repo = new RepositoryBase<Message>(_db);
            var model = repo.GetItemByExpression(x => x.MessageId == id);
            if (model == null) return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Message model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Unread");
            }
            catch (Exception ex)
            {
                return View(model).Error(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters, MessageStatus status)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<Message>(_db);
            var items = repo.GetItemsByExpression(w => w.Title.Contains(search), x => x.Title, parameters.Start, parameters.Length, out var recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsMessage();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.MessageId.ToString(),
                    item.Title,
                    EnumDescription.Get(item.Subject),
                    buttons.ToPagination(item.MessageId)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}