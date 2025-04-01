using System.Diagnostics.CodeAnalysis;
    
    namespace DevLab.Core.Communication;
    
    /// <summary>
    ///     Represents the result of an operation, including whether it succeeded and an associated list of errors if it
    ///     failed.
    /// </summary>
    public class Result
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the result is a success.</param>
        /// <param name="errors">The list of errors associated with a failed result.</param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when a successful result has errors or a failed result has no errors.
        /// </exception>
        protected Result(bool isSuccess, List<Error>? errors)
        {
            switch (isSuccess)
            {
                case true when errors?.Count > 0:
                    throw new InvalidOperationException("A successful result cannot have errors.");
                case false when errors?.Count == 0:
                    throw new InvalidOperationException("A failed result must have at least one error.");
                default:
                    IsSuccess = isSuccess;
                    Errors = errors ?? [];
                    break;
            }
        }
    
        /// <summary>
        ///     Indicates whether the result is a success.
        /// </summary>
        public bool IsSuccess { get; }
    
        /// <summary>
        ///     Indicates whether the result is a failure.
        /// </summary>
        public bool IsFailure => !IsSuccess;
    
        /// <summary>
        ///     The list of errors associated with a failed result.
        /// </summary>
        public List<Error> Errors { get; }
    
        /// <summary>
        ///     Creates a successful result.
        /// </summary>
        /// <returns>A new instance of the <see cref="Result"/> class representing a successful result.</returns>
        public static Result Success()
        {
            return new Result(true, null);
        }
    
        /// <summary>
        ///     Creates a failed result with the specified list of errors.
        /// </summary>
        /// <param name="errors">The list of errors associated with the failed result.</param>
        /// <returns>A new instance of the <see cref="Result"/> class representing a failed result.</returns>
        public static Result Failure(List<Error> errors)
        {
            return new Result(false, errors.ToList());
        }
    
        /// <summary>
        ///     Creates a successful result with a value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value of the successful result.</param>
        /// <returns>A new instance of the <see cref="Result{T}"/> class representing a successful result with a value.</returns>
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value, true, null);
        }
    
        /// <summary>
        ///     Creates a failed result with the specified list of errors.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="errors">The list of errors associated with the failed result.</param>
        /// <returns>A new instance of the <see cref="Result{T}"/> class representing a failed result with a value.</returns>
        public static Result<T> Failure<T>(List<Error> errors)
        {
            return new Result<T>(default, false, errors.ToList());
        }
    
        /// <summary>
        ///     Creates a result based on the value. If the value is null, it returns a failed result.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to create the result from.</param>
        /// <returns>A new instance of the <see cref="Result{T}"/> class representing the result.</returns>
        public static Result<T> Create<T>(T? value)
        {
            return value is not null ? Success(value) : Failure<T>([Error.NullValue]);
        }
    }
    
    /// <summary>
    ///     Represents the result of an operation that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Result<T> : Result
    {
        private readonly T? _value;
    
        /// <summary>
        ///     Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <param name="isSuccess">Indicates whether the result is a success.</param>
        /// <param name="errors">The list of errors associated with a failed result.</param>
        internal Result(T? value, bool isSuccess, List<Error>? errors)
            : base(isSuccess, errors)
        {
            _value = value;
        }
    
        /// <summary>
        ///     Gets the value of the result. Throws InvalidOperationException if the result is a failure.
        /// </summary>
        [NotNull]
        public T Value => _value ?? throw new InvalidOperationException("Result has no value");
    
        /// <summary>
        ///     Implicitly converts a value to a <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Result<T>(T? value)
        {
            return Create(value);
        }
    }
    
    /// <summary>
    ///     Extends the Result pattern with helper methods.
    /// </summary>
    public static class ResultWithErrorListExtensions
    {
        /// <summary>
        ///     Applies the specified action if the result is a success.
        /// </summary>
        /// <param name="result">The result to check.</param>
        /// <param name="action">The action to apply if the result is a success.</param>
        public static void OnSuccess(this Result result, Action action)
        {
            if (result.IsSuccess) action();
        }
    
        /// <summary>
        ///     Applies the specified action if the result is a failure.
        /// </summary>
        /// <param name="result">The result to check.</param>
        /// <param name="action">The action to apply if the result is a failure.</param>
        public static void OnFailure(this Result result, Action<List<Error>> action)
        {
            if (result.IsFailure) action(result.Errors);
        }
    
        /// <summary>
        ///     Applies the specified action with the value if the result is a success.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="result">The result to check.</param>
        /// <param name="action">The action to apply if the result is a success.</param>
        public static void OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess) action(result.Value);
        }
    
        /// <summary>
        ///     Applies the specified action if the result is a failure.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="result">The result to check.</param>
        /// <param name="action">The action to apply if the result is a failure.</param>
        public static void OnFailure<T>(this Result<T> result, Action<List<Error>> action)
        {
            if (result.IsFailure) action(result.Errors);
        }
    }