namespace blogest.domain.Constants;

public static class ErrorMessages
{
    public const string NotFound = "The requested resource was not found.";
    public const string Unauthorized = "You are not authorized to perform this action.";
    public const string Forbidden = "Access to this resource is forbidden.";
    public const string BadRequest = "The request was invalid or cannot be served.";
    public const string InternalServerError = "An unexpected error occurred on the server.";
    public const string ValidationError = "One or more validation errors occurred.";
    public const string DuplicateEntry = "The item already exists.";
    public const string Timeout = "The request timed out. Please try again.";
    public const string Conflict = "A conflict occurred with the current state of the resource.";
    public const string UnsupportedMediaType = "The media type is not supported.";
    public const string TooManyRequests = "Too many requests. Please slow down.";
    public const string AddCategoryAtLeast = "Add category atleast to add post";
}