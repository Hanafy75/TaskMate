namespace TaskMate.Application.Exceptions
{
    public class UnAuthorizedAccessEception : Exception
    {
        public UnAuthorizedAccessEception(string message) : base(message) { }
    }
}
