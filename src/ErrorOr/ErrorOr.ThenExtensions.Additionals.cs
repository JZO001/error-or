namespace ErrorOr.ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// Thens the do if asynchronous.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TInnerValue">The type of the inner value.</typeparam>
    /// <param name="errorOr">The error or.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="onValue">The on value.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ThenDoIfAsync<TValue, TInnerValue>(
        this Task<ErrorOr<TValue>> errorOr,
        Func<TValue, bool> condition,
        Func<TValue, Task<ErrorOr<TInnerValue>>> onValue)
    {
        var result = await errorOr.ConfigureAwait(false);
        return await result.ThenDoIfAsync(condition, onValue);
    }

    /// <summary>
    /// Thens the do if asynchronous.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TInnerValue">The type of the inner value.</typeparam>
    /// <param name="errorOr">The error or.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="onValue">The on value.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ThenDoIfAsync<TValue, TInnerValue>(
        this ErrorOr<TValue> errorOr,
        Func<TValue, bool> condition,
        Func<TValue, Task<ErrorOr<TInnerValue>>> onValue)
    {
        if (errorOr.IsError) return errorOr;
        if (!condition(errorOr.Value)) return errorOr;

        var innerValue = await errorOr.ThenAsync(onValue).ConfigureAwait(false);
        return innerValue.IsError ? innerValue.Errors : errorOr;
    }

    /// <summary>
    /// Thens the do if.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errorOr">The error or.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="onValue">The on value.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ThenDoIf<TValue>(
        this Task<ErrorOr<TValue>> errorOr,
        Func<TValue, bool> condition,
        Action<TValue> onValue)
    {
        return (await errorOr.ConfigureAwait(false)).ThenDoIf(condition, onValue);
    }

    /// <summary>
    /// Thens the do if.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errorOr">The error or.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="onValue">The on value.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static ErrorOr<TValue> ThenDoIf<TValue>(
        this ErrorOr<TValue> errorOr,
        Func<TValue, bool> condition,
        Action<TValue> onValue)
    {
        if (errorOr.IsError) return errorOr;
        if (!condition(errorOr.Value)) return errorOr;

        return errorOr.ThenDo(onValue);
    }
}
