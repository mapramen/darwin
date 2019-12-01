namespace Darwin.Data.Models.RequestContext
{
    public class UserContext
    {
        public string UserId { get; }

        public UserContext(string userId)
        {
            this.UserId = userId;
        }
    }
}
