using System;

namespace MalaebBooking.Application.Abstractions.Result;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success()
        => new(true, Error.None);

    public static Result Failure(Error error)
        => new(false, error);

    public static Result<T> Success<T>(T value)
        => new(true, Error.None, value);

    public static Result<T> Failure<T>(Error error)
        => new(false, error, default!);
}

public class Result<T> : Result
{
    private readonly T _value;

    public Result(bool isSuccess, Error error, T value)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException("The value of a failure result cannot be accessed.");

            return _value;
        }
    }

    public static Result<T> Success(T value)
        => new(true, Error.None, value);

    public static Result<T> Failure(Error error)
        => new(false, error, default!);
}