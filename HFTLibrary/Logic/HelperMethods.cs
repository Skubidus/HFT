namespace HFTLibrary.Logic;

public static class HelperMethods
{
    public static T Clone<T>(T original)
        where T : new()
    {
        T output = new();

        // TODO: Cloning implementieren mit Reflection

        return output;
    }
}