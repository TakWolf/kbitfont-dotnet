namespace KbitSpec.Utils;

internal interface ICopyable<out T>
{
    T Copy();

    T DeepCopy();
}

internal static class CopyUtil
{
    public static List<List<byte>> DeepCopyBitmap(List<List<byte>> source)
    {
        var copied = new List<List<byte>>(source.Count);
        foreach (var sourceRow in source)
        {
            copied.Add([.. sourceRow]);
        }
        return copied;
    }

    public static SortedDictionary<TKey, TValue> DeepCopySortedDictionary<TKey, TValue>(IDictionary<TKey, TValue> source)
        where TKey : notnull
        where TValue : ICopyable<TValue>
    {
        var copied = new SortedDictionary<TKey, TValue>();
        foreach (var (key, value) in source)
        {
            copied[key] = value.DeepCopy();
        }
        return copied;
    }
}
