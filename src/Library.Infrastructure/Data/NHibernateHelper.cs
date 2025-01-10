using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace Library.Infrastructure.Data
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Fluently.Configure()
                            .Database(SQLiteConfiguration.Standard.ConnectionString("Data Source=Library.db;"))
                            .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                            // Uncomment the line below to create the database schema
                            //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                            .BuildSessionFactory();

                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
