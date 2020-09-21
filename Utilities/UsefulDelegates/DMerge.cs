namespace HexCS.Core
{
    /// <summary>
    /// Takes two objects of the same type and merges them into a single object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ob1"></param>
    /// <param name="ob2"></param>
    /// <returns></returns>
    public delegate T DMerge<T>(T ob1, T ob2);
}
