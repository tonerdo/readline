using System;
using System.Collections.Generic;
using System.Linq;

using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

namespace System
{
    private class HistoryCollection : List<string>, IHistoryCollection
    {
      private int maxItems = int.MaxValue;

      public HistoryCollection(IEnumerable<string> collection) : base(collection)
      {
      }

      public int MaxItems
      {
        get { return maxItems; }
        set
        {
          if (value < maxItems && Count > value)
            RemoveRange(0, maxItems - value);
          maxItems = value;
        }
      }

      void IHistoryCollection.Add(params string[] collection)
      {
        var newItems = collection.Length > MaxItems
          ? collection.Skip(collection.Length - MaxItems).ToArray()
          : collection;

        if (Count + newItems.Length > MaxItems)
          RemoveRange(0, Count + newItems.Length - MaxItems);
        AddRange(newItems);
      }
    }
}
