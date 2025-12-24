using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.Data;
using Core.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Autofac
{
    public static class AutofacConfigurationDependency
    {
        public static void AddAutofacDependencyServices(this ContainerBuilder containerBuilder, IConfiguration configuration )
        {
            var currentAssembly = Assembly.GetExecutingAssembly();


            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                            .AssignableTo<IScopedDependency>()
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                            .AssignableTo<ISingletonDependency>()
                            .AsImplementedInterfaces()
                            .SingleInstance();

            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                            .AssignableTo<ITransientDependency>()
                            .AsImplementedInterfaces()
                            .InstancePerDependency();


            containerBuilder.RegisterGeneric(typeof(Repository<>))
                            .As(typeof(IRepository<>))
                            .InstancePerLifetimeScope();


            containerBuilder.Register(c =>
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                  .UseSqlServer(configuration.GetConnectionString("SqlServer"))
                                  .Options;
                return new ApplicationDbContext(options);
            }).AsSelf().InstancePerLifetimeScope();

            containerBuilder.RegisterType<TransactionManager>()
                            .As<ITransactionManager>()
                            .InstancePerLifetimeScope();



        }
    }
}
