using System;

namespace ReptileTracker.Commons;

public class Result<T> where T : new()
{
    private Result(bool isSuccess, Error error, T data)
    {
        if(isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }
        
        IsSuccess = isSuccess;
        Error = error;
        Data = data;
    }

    private Result(bool isSuccess, Error error)
    {
        if(isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }
        
        IsSuccess = isSuccess;
        Error = error;    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public T? Data { get; set; }
    public static Result<T> Success(T entity) => new Result<T>(true, Error.None, entity);
    public static Result<T> Success() => new Result<T>(true, Error.None);
    public static Result<T> Failure(Error error) => new Result<T>(false, error);
}