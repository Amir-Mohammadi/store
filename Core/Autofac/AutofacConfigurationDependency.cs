using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.Data;
using Core.Transaction;

namespace Core.Autofac
{
    public static class AutofacConfigurationDependency
    {
        public static void AddAutofacDependencyServices(this ContainerBuilder containerBuilder)
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



        }
    }
}
