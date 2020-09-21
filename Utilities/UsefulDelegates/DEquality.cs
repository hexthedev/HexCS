namespace HexCS.Core
{
    /// <summary>
    /// Determine whether two objects are equal
    /// </summary>
    /// <typeparam name="T">type of equality</typeparam>
    /// <param name="ob1">first object</param>
    /// <param name="ob2">second object</param>
    /// <returns>true if equal</returns>
    public delegate bool DEquality<T>(T ob1, T ob2);
}
