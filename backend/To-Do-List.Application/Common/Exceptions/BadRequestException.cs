namespace To_Do_List.Application.Common.Exceptions;

public class BadRequestException : Exception
{
    public IEnumerable<string> Errors { get; set; }

    public BadRequestException()
        : base()
    { }

    public BadRequestException(string message)
        : base(message)
    { }

    public BadRequestException(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    { }
}