namespace Darwin.WebApi.Middlewares
{
    using Darwin.Data.Models.RequestContext;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.JsonWebTokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UserContextMiddleware
    {
        private readonly RequestDelegate next;

        public UserContextMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                context.Items.Add(RequestContextConstants.ContextNames.User, ExtractUserContext(context));
            }
            await this.next(context);
        }

        private UserContext ExtractUserContext(HttpContext context)
        {
            var claims = context.User.Claims;
            return new UserContext(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
