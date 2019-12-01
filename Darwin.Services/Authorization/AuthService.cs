namespace Darwin.Services.Authorization
{
    using Darwin.Data.Models;
    using Google.Apis.Auth;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class AuthService : IAuthService
    {
        private IConfigurationRoot configuration { get; }
        private CloudTable identityProviderUsersTable { get; }
        private CloudTable usersTable { get; }

        public AuthService(IConfigurationRoot configuration, CloudTable identityProviderUsersTable, CloudTable usersTable)
        {
            this.configuration = configuration;
            this.identityProviderUsersTable = identityProviderUsersTable;
            this.usersTable = usersTable;
        }

        public Task<IdentityProviderUser> GetAuthorizedUser(IdentityProviderResponse identityProviderResponse)
        {
            string authorizationCode = identityProviderResponse.AuthorizationCode;
            return this.GetAuthorizedGoogleUser(authorizationCode);
        }

        private async Task<IdentityProviderUser> GetAuthorizedGoogleUser(string authorizationCode)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(authorizationCode, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string>
                    {
                        configuration["AppSettings:GoogleClientId"]
                    }
            });

            TableQuery<IdentityProviderUser> identityProviderUsersTableQuery = new TableQuery<IdentityProviderUser>()
            .Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, IdentityProvider.Google.ToString()),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, payload.Subject)
            ));

            var identityProviderUser = (await identityProviderUsersTable.ExecuteQuerySegmentedAsync(identityProviderUsersTableQuery, null)).SingleOrDefault();

            if (identityProviderUser == null)
            {
                var emailAddress = new MailAddress(payload.Email);
                var user = new User(Guid.NewGuid(), emailAddress.User, payload.GivenName, payload.FamilyName, emailAddress.Address, payload.Picture);
                await usersTable.ExecuteAsync(TableOperation.Insert(user));

                identityProviderUser = new IdentityProviderUser(IdentityProvider.Google, payload.Subject, user.UserId);
                await identityProviderUsersTable.ExecuteAsync(TableOperation.Insert(identityProviderUser));
            }

            return identityProviderUser;
        }
    }
}
