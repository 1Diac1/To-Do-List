namespace To_Do_List.Application.Common.Helpers;

public class BaseResponse
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Messages { get; set; } = new List<string>();
    
    public static BaseResponse Failure(string errorMessage) =>
        new BaseResponse { Succeeded = false, Messages = new[] { errorMessage } };

    public static BaseResponse Failure(IEnumerable<string> errors) =>
        new BaseResponse { Succeeded = false, Messages = errors };
    
    public static BaseResponse Success() =>
        new BaseResponse { Succeeded = true };
}