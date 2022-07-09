using System.Text.Json.Serialization;

namespace AuthServer.Shared.Dtos;

public class Response<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    
    [JsonIgnore]
    public bool IsSuccessful { get; set; }
    public ErrorDto Error { get; set; }

    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T>() { Data = data, StatusCode = statusCode, IsSuccessful = true};
    }
    
    public static Response<T> Success(int statusCode)
    {
        return new Response<T>() { Data = default, StatusCode = statusCode, IsSuccessful = true};
    }
    
    public static Response<T> Fail(ErrorDto error, int statusCode, bool isSuccessful)
    {
        return new Response<T>() { Error = error, StatusCode = statusCode, IsSuccessful = isSuccessful};
    }
    
    public static Response<T> Fail(string error, int statusCode, bool isSuccessful)
    {
        var errorDto = new ErrorDto(error,true);
        return new Response<T>() { Error = errorDto, StatusCode = statusCode, IsSuccessful = isSuccessful};
    }
}