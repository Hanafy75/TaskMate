namespace TaskMate.WebAPI.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? Message { get; }
        public IEnumerable<string>? Errors { get; }

        public ApiResponse(bool isSuccess, T? data, string? message, IEnumerable<string>? errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Errors = errors;
            Message = message;
        }

        public static ApiResponse<T> Success(T data, string message = "Request Processed Successfully")
        {
            return new ApiResponse<T>(true, data, message, null);
        }


        public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors=null)
        {
            return new ApiResponse<T>(false, default, message, errors);
        }
    }
}
