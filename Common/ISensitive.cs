using System;

namespace Axon_Job_App.Common;

public interface ISensitive<T>
{
     T desensitize();
}
