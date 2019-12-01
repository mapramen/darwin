namespace Darwin.Services.Authorization
{
    using Darwin.Data.Models;
    using System.Threading.Tasks;
    
    public interface IAuthService
    {
        Task<IdentityProviderUser> GetAuthorizedUser(IdentityProviderResponse identityProviderResponse);
    }
}
