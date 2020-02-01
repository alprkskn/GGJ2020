using System;
using System.Collections.Generic;

public static class CollectionExtensions
{
    public static T FindItem<T>(this HashSet<T> set, Func<T, bool> predicate) where T : class
    {
        foreach(var item in set)
        {
            if(predicate(item))
            {
                return item;
            }
        }

        return null;
    }
}
