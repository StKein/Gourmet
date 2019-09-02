using System;
using System.IO;
using NHibernate;
using NHibernate.Cfg;

namespace Gourmet.Models.NHibernate
{
    // Класс для соединения с базой
    // Используем, когда нужны не только стандартные get/list/save/update
    // Либо когда нужно подключение к нескольким таблицам по очереди
    public class DbOperator
    {
        private ISessionFactory SessionFactory;
        public ISession Session;

        public DbOperator()
        {
            Configuration Cfg = new Configuration();
            Cfg.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml"));
            this.SessionFactory = Cfg.BuildSessionFactory();
            this.Session = this.SessionFactory.OpenSession();
        }

        public void Close()
        {
            this.Session.Close();
            this.SessionFactory.Close();
        }
    }
}