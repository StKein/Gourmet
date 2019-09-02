using System.Web.Mvc;
using Gourmet.Models;
using Gourmet.Models.NHibernate;

namespace Gourmet.Controllers
{
    public class TablesController : Controller
    {
        private DbOperatorTyped<Table> Db;

        public TablesController()
        {
            this.Db = new DbOperatorTyped<Table>();
        }


        public ActionResult Index()
        {
            ViewBag.Title = "Столы ресторана";

            return View(this.Db.GetList());
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Новый стол";

            return View("Form", new Table());
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Изменение стола";
            Table table = this.Db.Get(id);

            if (table == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Form", table);
            }
        }

        public ActionResult Delete(int id)
        {
            this.Db.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Item(Table table)
        {
            if (ModelState.IsValid)
            {
                this.Db.Save(table);

                return RedirectToAction("Index");
            }

            return View("Form", table);
        }

        [HttpPost]
        public ActionResult SwitchSeated(int free, int id)
        {
            DbOperator db = new DbOperator();
            db.Session.BeginTransaction();

            Table table = db.Session.Get<Table>(id);
            table.IsFree = ( free > 0 );
            if (table.CurrentOrder > 0)
            {
                Order order = db.Session.Get<Order>(table.CurrentOrder);
                order.Status = -1;
                db.Session.Save(order);
                table.CurrentOrder = 0;
            }

            db.Session.Save(table);
            db.Session.Transaction.Commit();
            db.Close();

            return PartialView("TableRowInner", table);
        }
    }
}