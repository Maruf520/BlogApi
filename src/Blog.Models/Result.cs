using System;

namespace Blog.Models
{
    public class Result<T>
    {
        private Result(bool isSuccess, Error error, T data)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid Error", nameof(error));
            }
            IsSuccess = isSuccess;
            Error = error;
            Data = data;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public T Data { get; }

        public static Result<T> Success(T data) => new(true, Error.None, data);
        public static Result<T> Failure(string message) => new(false, new Error(message), default);
    }
    public sealed record Error(string Message)
    {
        public static readonly Error None = new(string.Empty);
    }
}
