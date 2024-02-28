
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonArray]
public class StackDataResponse
{
    public List<BlockData> Data;
}
