namespace Darwin.Data.Models
{
    using Darwin.Services.Authorization;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    public class IdentityProviderUser : TableEntity
    {
        public string UserId { get; set; }

        public IdentityProviderUser()
        {

        }

        public IdentityProviderUser(IdentityProvider identityProvider, string idTokenId, string userId)
        {
            this.PartitionKey = identityProvider.ToString();
            this.RowKey = idTokenId;
            this.UserId = userId;
        }
    }
}
