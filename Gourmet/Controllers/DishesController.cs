using System.Web.Mvc;
using Gourmet.Models;
using Gourmet.Models.NHibernate;

namespace Gourmet.Controllers
{
    public class DishesController : Controller
    {
        private DbOperatorTyped<Dish> Db;

        public DishesController()
        {
            this.Db = new DbOperatorTyped<Dish>();
        }


        public ActionResult Index()
        {
            ViewBag.Title = "Меню ресторана";

            return View(this.Db.GetList());
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Новое блюдо";

            return View("Form", new Dish());
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Изменить блюдо";
            Dish dish = this.Db.Get(id);

            if (dish == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Form", dish);
            }
        }

        public ActionResult Delete(int id)
        {
            this.Db.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Item(Dish dish)
        {
            if (ModelState.IsValid)
            {
                this.Db.Save(dish);
                Session["Dishes"] = null;

                return RedirectToAction("Index");
            }

            return View("Form", dish);
        }
    }
}