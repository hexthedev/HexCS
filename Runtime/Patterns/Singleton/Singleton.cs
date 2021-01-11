using System;

namespace HexCS.Core
{
    /// <summary>
    /// Implemntation of Singleton as an abstract class blinly folowing 
    /// https://csharpindepth.com/articles/singleton version six until I 
    /// have time to look closer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ASingleton<T>
        where T : new()
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(() => new T());

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static T Instance { get { return lazy.Value; } }

        private ASingleton()
        {
        }
    }
}
