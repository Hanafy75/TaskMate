namespace TaskMate.Application.Exceptions
{
    public class UserCreationException : Exception
    {
        public  IEnumerable<string>? Errors { get; } 

        public UserCreationException(IEnumerable<string> errors) : base("One or more errors occurred during user creation")
        {
            Errors = errors;
        }
    }
}
