namespace KioskApi;
public static class Arrays
{
    public static bool AreAllTheSameLength(params Array[] arrays)
    {
        return arrays.All(a => a.Length == arrays[0].Length);
    }
    
}

// public static class Utilities
// {
//     public static bool IsBetween<T>(this T item, T start, T end)
//     {
//         return Comparer<T>.Default.Compare(item, start) >= 0
//             && Comparer<T>.Default.Compare(item, end) <= 0;
//     }
// }