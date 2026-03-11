public class Error
 {
     public string Code { get; private set; }
     public string Description { get; private set; }
     public ErrorType Type { get; set; }
     private Error(string code, string description,ErrorType errorType )
     {
         Code = code;
         Description = description;
         Type = errorType;
     }


     ////Methods Factory
     public static Error Failure(string code = "Error.Failure", string description = "General Failure Has Occurred")
                 => new Error(code, description, ErrorType.Failure);
     public static Error NotFound(string code = "Error.NotFound", string description = "Requested Resource Was Not Found")
                 => new Error(code, description, ErrorType.NotFound);
     public static Error Unauthorized(string code = "Error.Unauthorized", string description = "Authentication Is Required")
                 => new Error(code, description, ErrorType.Unauthorized);
     public static Error Validation(string code = "Error.Validation", string description = "Validation Failed")
                 => new Error(code, description, ErrorType.Validation);
     public static Error Forbidden(string code = "Error.Forbidden", string description = "Access Is Forbidden")
                 => new Error(code, description, ErrorType.Forbidden);
     public static Error InvalidCredentials(string code = "Error.InvalidCredentials", string description = "Invalid Credentials Provided")
                 => new Error(code, description, ErrorType.InvalidCredentials);

 }