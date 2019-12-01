namespace Darwin.WebApi.AutofacRegistrations
{
    using Autofac;
    using Darwin.Data.Stores;
    using Darwin.Services.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage.Table;

    public class ServiceArtifactRegistrar
    {
        public static void RegisterAll(ContainerBuilder builder)
        {
            RegisterAuthServices(builder);
        }

        private static void RegisterAuthServices(ContainerBuilder builder)
        {
            builder
                .Register(c => new AuthService(
                    c.Resolve<IConfigurationRoot>(),
                    c.ResolveKeyed<CloudTable>(StoreConstants.TableNames.IdentityProviderUsers),
                    c.ResolveKeyed<CloudTable>(StoreConstants.TableNames.Users)))
                .As<IAuthService>()
                .InstancePerLifetimeScope();
        }
    }
}
