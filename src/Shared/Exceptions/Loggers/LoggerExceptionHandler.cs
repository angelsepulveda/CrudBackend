namespace Shared.Exceptions.Loggers;

public class LoggerExceptionHandler
{
    public static string LogInternalServerException(InternalServerException exception)
    {
        Log.Error(
            "ExceptionClass: {ExceptionClass}, error: {Error}",
            nameof(InternalServerException),
            JsonSerializer.Serialize(exception)
        );
        return exception.Message;
    }

    public static string LogException(Exception exception)
    {
        Log.Error(
            "ExceptionClass: {ExceptionClass}, error: {Error}",
            nameof(Exception),
            JsonSerializer.Serialize(exception)
        );
        return exception.Message;
    }
}
