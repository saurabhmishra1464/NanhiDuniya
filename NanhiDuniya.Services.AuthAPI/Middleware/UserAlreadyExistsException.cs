namespace NanhiDuniya.Services.AuthAPI.Middleware
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException()
        {
        }

        // Constructor with a custom message
        public UserAlreadyExistsException(string message)
            : base(message)
        {
        }

        // Constructor with a custom message and an inner exception
        public UserAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
