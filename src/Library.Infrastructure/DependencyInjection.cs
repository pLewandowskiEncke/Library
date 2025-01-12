using Library.Application.Behaviors;
using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using MediatR;
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
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}