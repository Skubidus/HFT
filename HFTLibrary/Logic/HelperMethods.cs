using System.Text.Json;

namespace HFTLibrary.Logic;

public static class HelperMethods
{
    public static T? CloneJson<T>(T original)
    {
        var serialize = JsonSerializer.Serialize(original);
        var output = JsonSerializer.Deserialize<T>(serialize);

        return output;
    }
}