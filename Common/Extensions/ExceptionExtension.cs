using System;

namespace axon_final_api.Common.Extensions;

public static class ExceptionExtension
{

     public static string sanitize(this Exception e, string? defaultError = null)
      {
         // todo:
         return e.GetBaseException().Message;
      }

}
