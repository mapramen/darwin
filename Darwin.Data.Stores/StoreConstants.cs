using System.Collections.Generic;

namespace Darwin.Data.Stores
{
    public class StoreConstants
    {
        public static class TableNames
        {
            public const string IdentityProviderUsers = "IdentityProviderUsers";

            public const string Users = "Users";

            public static IReadOnlyList<string> All = new List<string>
            {
                IdentityProviderUsers,
                Users
            };
        }
    }
}
