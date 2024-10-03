namespace NanhiDuniya.Services.AuthAPI.Middleware
{
    public class FailedToRevokeRefreshToken : Exception
    {
        public FailedToRevokeRefreshToken()
        {
        }

        // Constructor with a custom message
        public FailedToRevokeRefreshToken(string message)
            : base(message)
        {
        }

        // Constructor with a custom message and an inner exception
        public FailedToRevokeRefreshToken(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
