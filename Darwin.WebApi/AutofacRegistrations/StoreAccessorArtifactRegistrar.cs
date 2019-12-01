namespace Darwin.WebApi.AutofacRegistrations
{
    using Autofac;
    using Darwin.Data.Stores;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class StoreAccessorArtifactRegistrar
    {
        public static void RegisterAll(ContainerBuilder builder, IConfigurationRoot configuration)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("AzureStorageAccount"));
            RegisterCloudTables(builder, storageAccount);
        }

        private static void RegisterCloudTables(ContainerBuilder builder, CloudStorageAccount storageAccount)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            foreach (var tableName in StoreConstants.TableNames.All)
            {
                builder
                    .Register(c => tableClient.GetTableReference(tableName))
                    .Keyed<CloudTable>(tableName)
                    .InstancePerLifetimeScope();
            }
        }
    }
}
