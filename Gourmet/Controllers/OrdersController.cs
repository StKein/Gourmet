using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Gourmet.Models;
using Gourmet.Models.NHibernate;

namespace Gourmet.Controllers
{
    public class OrdersController : Controller
    {
        public ActionResult Index(int id = 0)
        {
            ViewBag.Title = "Заказы посетителей";

            // Если передан id заказа (при переходе от стола к его текущему заказу) - выводим только его
            if (id > 0)
            {
                try
                {
                    return View(new List<Order> { this.GetExistingOrderWithAdditionalInfo(id) });
                }
                catch (KeyNotFoundException){}
            }

            if( Session["Waiters"] == null ){
                this.SetWaitersNames();
            }
            Dictionary<int, string> waiters_names = (Dictionary<int, string>)Session["Waiters"];

            IList<Order> orders = (new DbOperatorTyped<Order>()).GetList();
            for( int i = 0; i < orders.Count; i++ )
            {
                if( waiters_names.ContainsKey(orders[i].Waiter)){
                    orders[i].WaiterName = waiters_names[orders[i].Waiter];
                }
                orders[i].MenuDescription = this.GetOrderMenuDescription(orders[i].Menu);
            }

            return View(orders);
        }

        public ActionResult Create(int id)
        {
            ViewBag.Title = "Оформление заказа";

            Order order = new Order();
            order.Table = id;
            order.Menu = "{}";

            // Проставляем случайного официанта
            IList<Person> waiters = this.GetWaitersList();
            int waiter_index = (new Random()).Next(waiters.Count);
            order.Waiter = waiters[waiter_index].Id;
            order.WaiterName = waiters[waiter_index].Name;
            Session["Waiter"] = order.WaiterName;

            return View("Form", order);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Изменение заказа";

            try
            {
                Order order = this.GetExistingOrderWithAdditionalInfo(id);
                Session["OrderMenu"] =
                    (new JavaScriptSerializer()).Deserialize<Dictionary<string, int>>(order.Menu);
                ViewBag.Dishes = order.MenuDescription;

                return View("Form", order);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("Index");
            }
        }

        // Выдаем меню заказа, для popup
        [HttpPost]
        public ActionResult Menu()
        {
            ViewBag.Dishes = Session["OrderMenu"];

            return PartialView( ( new DbOperatorTyped<Dish>() ).GetList() );
        }

        // Сохраняем список выбранных блюд в сессии
        // Также записываем его в ViewBag для представления
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDishes(FormCollection post_data)
        {
            Dictionary<string, int> post_data_processed;
            ViewBag.Dishes = this.GetOrderMenuDescription(post_data, out post_data_processed);
            Session["OrderMenu"] = post_data_processed;
            ViewBag.DishesJson = (new JavaScriptSerializer()).Serialize(post_data_processed);

            return PartialView("MenuShort");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Item(Order order)
        {
            if (ModelState.IsValid)
            {
                if (order.Id > 0)
                {
                    // Изменение заказа - просто апдейтим измененные данные
                    (new DbOperatorTyped<Order>()).Save(order);
                }
                else
                {
                    // Сохранение нового заказа
                    DbOperator db = new DbOperator();

                    db.Session.BeginTransaction();
                    int order_id = (int)db.Session.Save(order);
                    db.Session.Transaction.Commit();

                    // Проставляем ID созданного заказа у его стола
                    Table table = db.Session.Get<Table>(order.Table);
                    if (table != null)
                    {
                        table.CurrentOrder = order_id;
                        db.Session.BeginTransaction();
                        db.Session.Update(table);
                        db.Session.Transaction.Commit();
                    }

                    db.Close();
                }

                return RedirectToAction("Index");
            }
            if (order.WaiterName == null)
            {
                order.WaiterName = (string)Session["Waiter"];
            }

            return View("Form", order);
        }

        [HttpPost]
        public string SetStatus(int id, int status)
        {
            DbOperator db = new DbOperator();
            db.Session.BeginTransaction();

            Order order = db.Session.Get<Order>(id);
            order.Status = status;
            db.Session.Update(order);

            // Поскольку заказ завершен, обнуляем у его стола текущий заказ
            Table table = db.Session.Get<Table>(order.Table);
            if (table != null)
            {
                table.CurrentOrder = 0;
                db.Session.Update(table);
            }

            db.Session.Transaction.Commit();
            db.Close();

            return order.StatusDescription;
        }


        private IList<Person> GetWaitersList()
        {
            DbOperator db = new DbOperator();
            IList<Person> waiters =
                db.Session.CreateQuery("FROM Person WHERE Position = :required_position")
                        .SetString("required_position", "Официант")
                        .List<Person>();
            db.Close();

            return waiters;
        }
        
        // Сохраняем в сессии соотношения "ID блюда - название"
        private void SetDishesNames()
        {
            Dictionary<int, string> dishes_names = new Dictionary<int, string>();
            IList<Dish> dishes = (new DbOperatorTyped<Dish>()).GetList();
            foreach (Dish dish in dishes)
            {
                dishes_names.Add(dish.Id, dish.Name);
            }
            Session["Dishes"] = dishes_names;
        }

        // Сохраняем в сессии соотношения "ID официанта - ФИО"
        private void SetWaitersNames()
        {
            Dictionary<int, string> waiters_names = new Dictionary<int, string>();
            IList<Person> waiters = this.GetWaitersList();
            foreach (Person waiter in waiters)
            {
                waiters_names.Add(waiter.Id, waiter.Name);
            }
            Session["Waiters"] = waiters_names;
        }


        //  Получаем соотношения "название блюда - количество"
        //  На вход:
        //      - пришедшая форма меню заказа
        //      - словарь, куда будут записаны включенные блюда
        //          (для последующей перегонки в JSON и записи в базу)
        private Dictionary<string, int> GetOrderMenuDescription(
                FormCollection post_data, 
                out Dictionary<string, int> order_menu_for_db)
        {
            if (Session["Dishes"] == null)
            {
                this.SetDishesNames();
            }
            order_menu_for_db = new Dictionary<string, int>();
            Dictionary<string, int> order_menu = new Dictionary<string, int>();
            int current_key;
            int current_val;
            Dictionary<int, string> dishes_names = (Dictionary<int, string>)Session["Dishes"];
            foreach( string key in post_data )
            {
                if (int.TryParse(key, out current_key)
                        && int.TryParse(post_data[key], out current_val)
                        && current_val > 0)
                {
                    order_menu.Add(dishes_names[current_key], current_val);
                    order_menu_for_db.Add(key, current_val);
                }
            }

            return order_menu;
        }

        // Возвращаемые данные аналогичны методу выше
        // Отличие - на вход JSON выбранных блюд (из базы)
        private Dictionary<string, int> GetOrderMenuDescription(string order_menu_json)
        {
            if (Session["Dishes"] == null)
            {
                this.SetDishesNames();
            }
            Dictionary<int, string> dishes_names = (Dictionary<int, string>)Session["Dishes"];
            Dictionary<string, int> order_menu = new Dictionary<string, int>();
            Dictionary<string, int> order_menu_native = 
                (new JavaScriptSerializer()).Deserialize<Dictionary<string, int>>(order_menu_json);
            int current_key;
            foreach( KeyValuePair<string,int> order_dish in order_menu_native ){
                if (int.TryParse(order_dish.Key, out current_key) 
                                && dishes_names[current_key] != null)
                {
                    order_menu[dishes_names[current_key]] = order_dish.Value;
                }
            }

            return order_menu;
        }

        // Возвращает данные по заказу из базы
        // А также проставленные ФИО официанта и набор выбранных блюд для вывода
        // Если заказ не найдет, генерирует KeyNotFoundException
        private Order GetExistingOrderWithAdditionalInfo(int id)
        {
            DbOperator db = new DbOperator();
            Order order = db.Session.Get<Order>(id);
            if (order == null)
            {
                throw new KeyNotFoundException();
            }
            order.WaiterName = (db.Session.Get<Person>(order.Waiter)).Name;
            db.Close();
            order.MenuDescription = this.GetOrderMenuDescription(order.Menu);

            return order;
        }
    }
}