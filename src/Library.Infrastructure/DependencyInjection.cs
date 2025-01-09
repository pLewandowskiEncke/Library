using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(NHibernateHelper.SessionFactory);
            services.AddScoped(factory => NHibernateHelper.OpenSession());
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}