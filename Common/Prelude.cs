using System;
using System.Text;

namespace Axon_Job_App.Common;

public static class Prelude
{
    public static string reverse(this string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return s;
        var sb = new StringBuilder(s.Length);
        for(var i = s.Length-1; i >= 0; i--)
        {
            sb.Append(s[i]);
        }
        return sb.ToString();
    }
    public static (IEnumerable<T> leftOnly, IEnumerable<T> inBoth, IEnumerable<U> rightOnly) split<T, U>(this IEnumerable<T> left, IEnumerable<U> right, Func<T, U, bool> isMatch)
    {
        // todo: optimize this
        var leftOnly = left.Where(x => !right.Any(y => isMatch(x, y)));
        var inBoth = left.Where(x => right.Any(y => isMatch(x, y)));
        var rightOnly = right.Where(x => !left.Any(y => isMatch(y, x)));
        return (leftOnly, inBoth, rightOnly);
    }

    public static X pipe<T, X>(this T x, Func<T, X> f) => f(x);
    public static async Task<X> pipe<T, X>(this Task<T> x, Func<T, X> f)
    {
        var y = await x;
        return f(y);
    }
    public static async Task<X> pipe<T, X>(this Task<T> x, Func<T, Task<X>> f)
    {
        var y = await x;
        return await f(y);
    }
    public static async Task<(T? result, Exception? error)> @catch<T>(this Task<T> x)
    {
        try
        {
            return (await x, default);
        }
        catch (Exception e)
        {
            return (default, e);
        }
    }
    public static void pipe<T>(this T x, Action<T> f) => f(x);

    public static IEnumerable<X> map<T, X>(this IEnumerable<T> xs, Func<T, X> f) =>
        xs.Select(f);
    public static async Task<X> mapIfNotNull<T, X>(this Task<T> x, Func<T, X> f, X nullValue = default)
    {
        var y = await x;
        if (y == null) return nullValue;
        return f(y);
    }
    public static void iter<T>(this IEnumerable<T> xs, Action<T> f)
    {
        foreach (var x in xs) f(x);
    }
    public static void iter<T>(this IEnumerable<T> xs, Action<T, int> f)
    {
        var i = 0;
        foreach (var x in xs) f(x, i++);
    }
    public static async Task iter<T>(this IEnumerable<T> xs, Func<T, Task> f)
    {
        foreach (var x in xs)
            await f(x);
    }
    public static async Task iter<T>(this IEnumerable<T> xs, Func<T, int, Task> f)
    {
        var i = 0;
        foreach (var x in xs)
            await f(x, i++);
    }
    public static async Task<(T? value, Exception? error)> tryCatch<T>(this Task<T> task)
    {
        try
        {
            return (await task, default);
        }
        catch (Exception e)
        {
            return (default, e);
        }
    }
}
