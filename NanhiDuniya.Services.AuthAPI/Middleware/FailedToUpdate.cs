namespace NanhiDuniya.Services.AuthAPI.Middleware
{
    public class FailedToUpdate : Exception
    {
        public FailedToUpdate()
        {
        }

        // Constructor with a custom message
        public FailedToUpdate(string message)
            : base(message)
        {
        }

        // Constructor with a custom message and an inner exception
        public FailedToUpdate(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
