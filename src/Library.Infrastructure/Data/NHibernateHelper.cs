using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
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
                    try
                    {
                        _sessionFactory = Fluently.Configure()
                            .Database(SQLiteConfiguration.Standard
                                .ConnectionString("Data Source=Library.db;Version=3;New=True;"))
                            .Mappings(m => m.FluentMappings
                                .AddFromAssembly(Assembly.GetExecutingAssembly()))
                            .ExposeConfiguration(cfg => new SchemaExport(cfg)
                                .Create(false, true))
                            .BuildSessionFactory();
                    }
                    catch (FluentConfigurationException ex)
                    {
                        Console.WriteLine($"NHibernate configuration error: {ex.Message}");
                        foreach (var reason in ex.PotentialReasons)
                        {
                            Console.WriteLine($"Potential reason: {reason}");
                        }
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                        }
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"NHibernate configuration error: {ex.Message}");
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                        }
                        throw;
                    }
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
