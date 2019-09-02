using System;
using System.Collections.Generic;
using System.IO;
using NHibernate;
using NHibernate.Cfg;

namespace Gourmet.Models.NHibernate
{
    // Строго типизированный класс подключения к базе
    // Стандартный набор запросов: get/list/save/update
    // Использовать, когда нужно просто достать/записать данные по одной таблице
    public class DbOperatorTyped<T>
    {
        private Configuration Cfg;
        private ISessionFactory SessionFactory;
        private ISession Session;

        public DbOperatorTyped()
        {
            this.Cfg = new Configuration();
            this.Cfg.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml"));
        }


        public T Get(int id)
        {
            this.Start();
            T item = this.Session.Get<T>(id);
            this.End();

            return item;
        }
        
        public IList<T> GetList()
        {
            this.Start();
            IList<T> items = this.Session.CreateCriteria(typeof(T)).List<T>();
            this.End();

            return items;
        }

        public void Delete(int id)
        {
            this.Start(true);
            this.Session.Delete(this.Session.Get<T>(id));
            this.End(true);
        }

        public void Save(T item)
        {
            this.Start(true);
            this.Session.SaveOrUpdate(item);
            this.End(true);
        }


        private void Start(bool transaction_required = false)
        {
            this.SessionFactory = this.Cfg.BuildSessionFactory();
            this.Session = this.SessionFactory.OpenSession();
            if (transaction_required)
            {
                this.Session.BeginTransaction();
            }
        }

        private void End(bool transaction_required = false)
        {
            if (transaction_required)
            {
                this.Session.Transaction.Commit();
            }
            this.Session.Close();
            this.SessionFactory.Close();
        }
    }
}