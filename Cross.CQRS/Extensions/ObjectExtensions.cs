namespace Cross.CQRS.Extensions;

public static class ObjectExtensions
{
    public static int GetObjectSize(this object obj)
    {
        var jsonString = JsonSerializer.Serialize(obj);
        var array = Encoding.UTF8.GetBytes(jsonString);
        return array.Length;
    }
}