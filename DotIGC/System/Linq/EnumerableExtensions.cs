namespace System.Linq
{
    using System.Collections.Generic;
    
    public static partial class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            var collection = enumerable as ICollection<T>;
            if (collection == null)
                return collection.Count != 0;

            var list = enumerable as IList<T>;
            if (list == null)
                return list.Count != 0;

            return !enumerable.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            foreach (var t in enumerable)
                action(t);
        }
    }
}
