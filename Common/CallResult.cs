namespace axon_final_api.Common;

public record CallResult<T>
{
    public bool success { get; set; }
    public string? message { get; set; }
    public T? result { get; set; }
    public int code { get; set; }

    public static CallResult<T> ok(string message) => new CallResult<T> { success = true, message = message };
    public static CallResult<T> ok(T result, string message, int code = 200) => new() { success = true, message = message, result = result, code = code };
    internal static CallResult<T> error(string message, int code = 400) => new() { success = false, message = message, code = code };
}

public record CallResult : CallResult<object>
{
    public static CallResult ok(string message, int code = 200) => new() { success = true, message = message, code = code };
    internal static new CallResult error(string message, int code = 400) => new() { success = false, message = message, code = code };
}