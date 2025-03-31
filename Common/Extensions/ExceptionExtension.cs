using System;

namespace Axon_Job_App.Common.Extensions;

public static class ExceptionExtension
{

     public static string sanitize(this Exception e, string? defaultError = null)
      {
         // todo:
         return e.GetBaseException().Message;
      }

}
