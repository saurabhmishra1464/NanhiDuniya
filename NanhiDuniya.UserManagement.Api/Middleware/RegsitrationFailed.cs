namespace NanhiDuniya.UserManagement.Api.Middleware
{
    public class RegsitrationFailed : Exception
    {
        public RegsitrationFailed()
        {
        }

        // Constructor with a custom message
        public RegsitrationFailed(string message)
            : base(message)
        {
        }

        // Constructor with a custom message and an inner exception
        public RegsitrationFailed(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
