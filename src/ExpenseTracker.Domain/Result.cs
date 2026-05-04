namespace ExpenseTracker.Domain;

/// <summary>
/// Результат операції з інформацією про успіх або помилку
/// </summary>
public abstract record Result
{
    public sealed record Success : Result;
    public sealed record Failure(string ErrorMessage) : Result;

    public static Result Ok() => new Success();
    public static Result Fail(string message) => new Failure(message);

    public T Match<T>(Func<Success, T> onSuccess, Func<Failure, T> onFailure)
    {
        return this switch
        {
            Success s => onSuccess(s),
            Failure f => onFailure(f),
            _ => throw new InvalidOperationException("Невідомий тип результату")
        };
    }

    public void Match(Action<Success> onSuccess, Action<Failure> onFailure)
    {
        switch (this)
        {
            case Success s:
                onSuccess(s);
                break;
            case Failure f:
                onFailure(f);
                break;
        }
    }
}

/// <summary>
/// Результат операції з даними
/// </summary>
public abstract record Result<T>
{
    public sealed record Success(T Value) : Result<T>;
    public sealed record Failure(string ErrorMessage) : Result<T>;

    public static Result<T> Ok(T value) => new Success(value);
    public static Result<T> Fail(string message) => new Failure(message);

    public TR Match<TR>(Func<Success, TR> onSuccess, Func<Failure, TR> onFailure)
    {
        return this switch
        {
            Success s => onSuccess(s),
            Failure f => onFailure(f),
            _ => throw new InvalidOperationException("Невідомий тип результату")
        };
    }
}
