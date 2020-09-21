namespace HexCS.Core
{
    /// <summary>
    /// Maps an object of type T to another object of the same type
    /// </summary>
    /// <typeparam name="T">Type of map</typeparam>
    /// <param name="original">The original object</param>
    /// <returns>An object mapped to some new object</returns>
    public delegate T DMap<T>(T original);

    /// <summary>
    /// Maps object of type TOrig to new object of type TMap
    /// </summary>
    /// <typeparam name="TOrig">Type of original object</typeparam>
    /// <typeparam name="TMap">Type of mapped object</typeparam>
    /// <param name="original">The original object to map</param>
    /// <returns>The new mapped object</returns>
    public delegate TMap DMap<TOrig, TMap>(TOrig original);
}
