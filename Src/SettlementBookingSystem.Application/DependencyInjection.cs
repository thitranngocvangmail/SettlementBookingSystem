using AutoMapper.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettleBookingSystem.Domain.Core;
using SettleBookingSystem.Domain.Entities;
using SettleBookingSystem.Domain.SettlementBooking;
using SettlementBookingSystem.Application.Behaviours;
using SettlementBookingSystem.SqlRepository;
using SettlementBookingSystem.SqlRepository.SettlementBooking;
using System.Linq;
using System.Reflection;

namespace SettlementBookingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SettlementBookingSystem");

            services.AddDbContext<SettlementBookingDbContext>(options => options.UseInMemoryDatabase(connectionString));


            var assembly = typeof(DependencyInjection).Assembly;
            services.AddAutoMapper(assembly);

            services.AddFluentValidation(assembly);
            services.AddMediatR(cfg => cfg.AsScoped(), assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
            services.AddSingleton(typeof(IGuidGenerator), typeof(SequentialGuidGenerator));
            services.AddScoped(typeof(ISettlementBookingRepository), typeof(SettlementBookingRepository));
            services.AddScoped(typeof(ISettlementBookingService), typeof(SettlementBookingService));


            

            return services;
        }

        private static void AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            var validatorType = typeof(IValidator<>);

            var validatorTypes = assembly
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == validatorType))
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var requestType = validator.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                var validatorInterface = validatorType
                    .MakeGenericType(requestType);

                services.AddTransient(validatorInterface, validator);
            }
        }
    }
}
