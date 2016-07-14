using Autofac;
using Autofac.Integration.Mvc;
using Parkheesung.Domain.Abstract;
using Parkheesung.Domain.Repository;
using Parkheesung.WebUI.Abstract;
using System.Reflection;

namespace Parkheesung.WebUI.Models
{
    public class AutofacConfig
    {
        public static IContainer Create(Assembly assembly)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(assembly);

            builder.RegisterType<EntityRepository>().As<IRepository>();
            builder.RegisterType<SiteUtility>().As<ISiteUtility>();

            return builder.Build();
        }
    }
}
