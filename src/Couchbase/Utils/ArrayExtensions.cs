using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase.Utils
{
    public static class ArrayExtensions
    {
        public static bool IsJson(this byte[] theArray, int startIndex, int endIndex)
        {
            if (endIndex < theArray.Length)
            {
                return false;
            }
            return (theArray.Length > 1 && theArray[startIndex] == 0x5b && theArray[endIndex] == 0x5d) ||
                   (theArray.Length > 1 && theArray[startIndex] == 0x7b && theArray[endIndex] == 0x7d);
        }
    }
}
