using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexCS.Core
{
    public static class UTObject
    {
        public static bool TryAs<T>(this object ob, out T val)
        {
            if (!(ob is T))
            {
                val = default;
                return false;
            }

            val = (T)ob;
            return true;
        }
    }
}