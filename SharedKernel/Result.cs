namespace eventify.SharedKernel
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Successful result must not have an error.");

            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Failed result must have an error message.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, string.Empty);
        public static Result Failure(string error) => new(false, error);

        public static Result<T> Success<T>(T value) => Result<T>.Success(value);
        public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value => IsSuccess 
            ? _value 
            : throw new InvalidOperationException("Cannot access Value when result is a failure.");

        protected internal Result(T value, bool isSuccess, string error) 
            : base(isSuccess, error)
        {
            _value = value;
        }

        public static Result<T> Success(T value) => new(value, true, string.Empty);
        public static new Result<T> Failure(string error) => new(default!, false, error);
    }
}
