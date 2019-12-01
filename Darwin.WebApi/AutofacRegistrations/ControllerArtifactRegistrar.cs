namespace Darwin.WebApi.AutofacRegistrations
{
    using Autofac;
    using Autofac.Features.AttributeFilters;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class ControllerArtifactRegistrar
    {
        public static void RegisterAll(ContainerBuilder builder)
        {
            var controllers = typeof(Startup).Assembly.GetTypes()
                .Where(t => t.BaseType == typeof(ControllerBase))
                .ToArray();

            builder.RegisterTypes(controllers).WithAttributeFiltering();
        }
    }
}
