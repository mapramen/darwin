namespace Darwin.Data.Models.RequestContext
{
    using Microsoft.AspNetCore.Http;

    public static class RequestContextExtensions
    {
        public static UserContext GetUserContext(this HttpContext context)
        {
            return context.Items[RequestContextConstants.ContextNames.User] as UserContext;
        }
    }
}
