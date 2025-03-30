using System;

namespace axon_final_api.Common;

public interface ISensitive<T>
{
     T desensitize();
}
