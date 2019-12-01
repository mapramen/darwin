namespace Darwin.WebApi.AutofacRegistrations
{
    using Autofac;
    using Microsoft.Extensions.Configuration;

    public class ConfigurationArtifactRegistrar
    {
        public static void RegisterAll(ContainerBuilder builder, IConfigurationRoot configuration)
        {
            builder
                .Register(c => configuration)
                .As<IConfigurationRoot>()
                .InstancePerLifetimeScope();
        }
    }
}
