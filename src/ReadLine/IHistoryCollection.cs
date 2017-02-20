using System;
using System.Collections.Generic;

namespace Internal.ReadLine
{
    public interface IHistoryCollection : IList<string>
    {
      int MaxItems { get; set; }
      void Add(params string[] collection);
    }
}