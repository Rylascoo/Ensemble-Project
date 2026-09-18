namespace Kymaean.Application;

public enum ProductAccessFailureKind
{
    Incompatible,
    Invalid
}

public sealed class ProductAccessResult<T>
{
    private readonly T? _value;
    private readonly ProductAccessFailureKind? _failureKind;

    private ProductAccessResult(T value)
    {
        _value = value;
        IsSuccess = true;
    }

    private ProductAccessResult(ProductAccessFailureKind failureKind)
    {
        _failureKind = failureKind;
        IsSuccess = false;
    }

    public bool IsSuccess { get; }

    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "A failed Product access result has no value.");

    public ProductAccessFailureKind FailureKind =>
        !IsSuccess
            ? _failureKind!.Value
            : throw new InvalidOperationException(
                "A successful Product access result has no failure kind.");

    public static ProductAccessResult<T> Success(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new ProductAccessResult<T>(value);
    }

    public static ProductAccessResult<T> Failure(
        ProductAccessFailureKind failureKind)
    {
        if (!Enum.IsDefined(failureKind))
        {
            throw new ArgumentOutOfRangeException(nameof(failureKind));
        }

        return new ProductAccessResult<T>(failureKind);
    }
}
