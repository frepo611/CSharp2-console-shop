namespace ConsoleShoppen.Classes;
/// <summary>
/// Provides extension methods for lists.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Shuffles the elements of the specified list in place. Uses the Fisher-Yates algorithm. https://jamesshinevar.medium.com/shuffle-a-list-c-fisher-yates-shuffle-32833bd8c62d
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    /// <exception cref="ArgumentNullException">Thrown when the list is null.</exception>
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Shared.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
