namespace Cross.CQRS.Extensions;

public static class ObjectExtensions
{
    public static int GetObjectSize(this object obj)
    {
        byte[] array;

        try
        {
            var jsonString = JsonSerializer.Serialize(obj);
            array = Encoding.UTF8.GetBytes(jsonString);
        }
        catch (InvalidOperationException)
        {
            // Do ignore. InvalidOperationException when try to serialize obj with Stream field
            return int.MaxValue;
        }
        catch (NotSupportedException)
        {
            // Do ignore. Serialization and deserialization of 'System.Type' instances is not supported. Path: $.EntitySelector.Type.
            return int.MaxValue;
        }

        return array.Length;
    }
}
