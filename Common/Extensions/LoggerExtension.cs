using System;

namespace Axon_Job_App.Common.Extensions;

public static class LoggerExtension
{
    public static void logCommandError<T, X>(this ILogger<T> logger, X command, Exception e, bool logRequest = true)
    {
        if (logRequest)
        {
        if (command is ISensitive<X> s)
        {
            command = s.desensitize();
        }
        logger.LogError(e, "{action} with request @{command} failed with: {error}", typeof(X).Name, command, e.GetBaseException().Message);
        }
        else
        {
        logger.LogError(e, "{action} failed with: {error}", typeof(X).Name, e.GetBaseException().Message);
        }
    }

      public static void logActionError<T, TInput>(this ILogger<T> logger, TInput input, Exception e, bool logRequest = true, string? action = null)
      {
         action ??= typeof(TInput).Name;
         if (logRequest)
         {
            if (input is ISensitive<TInput> s)
            {
               input = s.desensitize();
            }
            logger.LogError(e, "{action} with request {input} failed with: {error}", action, input, e.sanitize());
         }
         else
         {
            logger.LogError(e, "{action} failed with: {error}", action, e.sanitize());
         }
      }
      public static void logActionError<T, TInput>(this ILogger<T> logger, string username, TInput input, Exception e, bool logRequest = true, string? action = null)
      {
         action ??= typeof(TInput).Name;
         if (logRequest)
         {
            if (input is ISensitive<TInput> s)
            {
               input = s.desensitize();
            }
            logger.LogError(e, "[{user}] {action} with request @{input} failed with: {error}", username, action, input, e.GetBaseException().Message);
         }
         else
         {
            logger.LogError(e, "[{user}] {action} failed with: {error}", username, action, e.GetBaseException().Message);
         }
      }
      public static void logActionInfo<T, TInput, TOutput>(this ILogger<T> logger, string username, TInput input, TOutput output, bool logRequest = true, string? message = null, string? action = null)
      {
         action ??= typeof(TInput).Name;
         if (logRequest)
         {
            if (input is ISensitive<TInput> s)
            {
               input = s.desensitize();
            }
            if (output is ISensitive<TOutput> o)
            {
               output = o.desensitize();
            }
            if (string.IsNullOrWhiteSpace(message))
               logger.LogInformation("[{user}] {action} with input @{input} completed with: {output}", username, action, input, output);
            else
               logger.LogInformation("[{user}] {action} with input @{input} completed with: {output}.\n{message}", username, action, input, output, message);
         }
         else if (string.IsNullOrWhiteSpace(message))
         {
            logger.LogInformation("[{user}] {action} completed successfully", username, action);
         }
         else
         {
            logger.LogInformation("[{user}] {action} completed with: {message}", username, action, message);
         }
      }
      public static void logActionInfo<T, TInput, TOutput>(this ILogger<T> logger, TInput input, TOutput output, bool logRequest = true, string? message = null, string? action = null)
      {
         action ??= typeof(TInput).Name;
         if (logRequest)
         {
            if (input is ISensitive<TInput> s)
            {
               input = s.desensitize();
            }
            if (output is ISensitive<TOutput> o)
            {
               output = o.desensitize();
            }
            if (string.IsNullOrWhiteSpace(message))
               logger.LogInformation("{action} with input @{input} completed with: {output}", action, input, output);
            else
               logger.LogInformation("{action} with input @{input} completed with: {output}.\n{message}", action, input, output, message);
         }
         else if (string.IsNullOrWhiteSpace(message))
         {
            logger.LogInformation("{action} completed successfully", action);
         }
         else
         {
            logger.LogInformation("{action} completed with: {message}", typeof(TInput).Name, message);
         }
      }
   

}
