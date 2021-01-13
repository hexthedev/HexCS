using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HexCS.Reflection
{
    /// <summary>
    /// Utilities used during refelection when handling assembly objects
    /// </summary>
    public static class UTAssembly
    {
        /// <summary>
        /// Gets all distinct types in the given assemblies. Assemblies that appear twice will be 
        /// reporcessed (so it's inefficent to pass the same assembly twice)
        /// </summary>
        /// <param name="assemblies">assemblies to pull tpyes from</param>
        /// <returns></returns>
        public static Type[] GetTypesFromAssemblies(params Assembly[] assemblies)
        {
            List<Type> types = new List<Type>();

            foreach (Assembly a in assemblies)
            {
                foreach (Type t in a.GetTypes())
                {
                    types.Add(t);
                }
            }

            return types.Distinct().ToArray();
        }

        /// <summary>
        /// Returns all types from an assembly after testing each type against th eprovided filters
        /// </summary>
        /// <param name="assembly">assembly to get types from</param>
        /// <param name="filters">filters to use</param>
        /// <returns>types that return true on all filters</returns>
        public static Type[] GetTypesFiltered(this Assembly assembly, params Predicate<Type>[] filters)
        {
            return assembly.GetTypes().Where(
                t =>
                {
                    foreach (Predicate<Type> filter in filters)
                    {
                        if (!filter(t))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            ).ToArray();
        }

        /// <summary>
        /// Gets all available namespaces in an Assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string[] GetAllNamespaces(this Assembly assembly)
        {
            HashSet<string> namespaces = new HashSet<string>();

            foreach(Type t in assembly.GetTypes())
            {
                namespaces.Add(t.Namespace);
            }

            return namespaces.ToArray();
        } 
    }
}
