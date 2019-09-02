using System.Web.Mvc;
using Gourmet.Models;
using Gourmet.Models.NHibernate;

namespace Gourmet.Controllers
{
    public class PersonnelController : Controller
    {
        private DbOperatorTyped<Person> Db;

        public PersonnelController()
        {
            this.Db = new DbOperatorTyped<Person>();
        }


        public ActionResult Index()
        {
            ViewBag.Title = "Персонал ресторана";

            return View(this.Db.GetList());
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Новый сотрудник";

            return View("Form", new Person());
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Изменение данных сотрудника";
            Person person = this.Db.Get(id);

            if (person == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Form", person);
            }
        }

        public ActionResult Delete(int id)
        {
            this.Db.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Item(Person person)
        {
            if (ModelState.IsValid)
            {
                this.Db.Save(person);
                if (person.Position == "Официант")
                {
                    Session["Waiters"] = null;
                }

                return RedirectToAction("Index");
            }

            return View("Form", person);
        }
    }
}