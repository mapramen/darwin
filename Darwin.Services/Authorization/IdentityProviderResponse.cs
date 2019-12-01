namespace Darwin.Services.Authorization
{
    public class IdentityProviderResponse
    {
        public IdentityProvider Provider { get; set; }
        public string AuthorizationCode { get; set; }
    }
}
