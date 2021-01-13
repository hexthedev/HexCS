using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HexCS.Core
{
    /// <summary>
    /// General utilties for Type objects
    /// </summary>
    public static class UTType
    {
        /// <summary>
        /// Returns all distinct namespaces from an array of types based on a namespaces
        /// level which represents how many steps in the namespace you're interested in. 
        /// 
        /// Example: the namespace a.b.c.d with namespaceLevel 2 is a.b
        /// </summary>
        /// <param name="types"></param>
        /// <param name="namespaceLevels"></param>
        /// <returns>"_NoNamespace" if namespace == null </returns>
        public static string[] NamespacesFromTypes(Type[] types, int namespaceLevels = 0)
        {
            HashSet<string> namespaces = new HashSet<string>();

            foreach (Type t in types)
            {
                string key = string.Empty;

                if (namespaceLevels <= 0) key = t.Namespace;
                else
                {
                    if (t.Namespace == null) key = "_NoNamespace";
                    else key = t.Namespace.SeparatorLevelPrune('.', namespaceLevels);
                }

                namespaces.Add(key);
            }

            return namespaces.ToArray();
        }



        /// <summary>
        /// A Map that removed the + characters that comes up in private class names and replaces them with a .
        /// </summary>
        public static string Map_PrivateTypeName(string s) => s.Replace("+", ".");


        /// <summary>
        /// <para>Test is a Type object is ETest using AND comparision</para>
        /// </summary>
        /// <param name="typeTest"></param>
        /// <returns></returns>
        public static Predicate<Type> Test_And(params ETypeTest[] typeTest)
        {
            IEnumerable<Predicate<Type>> predicates = typeTest.Select(s => s.GetTest());

            return t =>
            {
                foreach (Predicate<Type> test in predicates)
                {
                    if (!test(t)) return false;
                }

                return true;
            };
        }

        /// <summary>
        /// <para>Test is a Type object is ETest using OR comparision</para>
        /// </summary>
        /// <param name="typeTest"></param>
        /// <returns></returns>
        public static Predicate<Type> Test_Or(params ETypeTest[] typeTest)
        {
            IEnumerable<Predicate<Type>> predicates = typeTest.Select(s => s.GetTest());

            return t =>
            {
                foreach (Predicate<Type> test in predicates)
                {
                    if (test(t)) return true;
                }

                return false;
            };
        }

        /// <summary>
        /// <para> Returns a function that tests a type object. Returns true if Type meets criteria for ETypeTest</para>
        /// </summary>
        /// <param name="typeTest"></param>
        /// <returns></returns>
        public static Predicate<Type> GetTest(this ETypeTest typeTest)
        {
            switch (typeTest)
            {
                case ETypeTest.ABSTRACT:
                    return t => t.IsAbstract;
                case ETypeTest.SEALED:
                    return t => t.IsSealed;
                case ETypeTest.GENERIC:
                    return t => t.FullName.Contains("`");
                // According to https://stackoverflow.com/questions/2483023/how-to-test-if-a-type-is-anonymous
                case ETypeTest.ANONYMOUS:
                    return t => Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute), false)
                                && t.IsGenericType
                                && t.Name.Contains("AnonymousType")
                                && (t.Name.StartsWith("<>") || t.Name.StartsWith("VB$"))
                                && t.Attributes.HasFlag(TypeAttributes.NotPublic);
                case ETypeTest.TOBIAS_VALUE_TYPE:
                    return t => t.Name.StartsWith("S") && t.IsValueType;
                case ETypeTest.INTERFACE:
                    return t => t.IsInterface;
                case ETypeTest.ENUM:
                    return t => t.IsEnum;
                case ETypeTest.STATIC:
                    return t => t.IsSealed && t.IsAbstract;
                // Below 4 taken from https://stackoverflow.com/questions/4971213/how-to-use-reflection-to-determine-if-a-class-is-internal
                case ETypeTest.INTERNAL:
                    return t => !t.IsVisible
                                && !t.IsPublic
                                && t.IsNotPublic
                                && !t.IsNested
                                && !t.IsNestedPublic
                                && !t.IsNestedFamily
                                && !t.IsNestedPrivate
                                && !t.IsNestedAssembly
                                && !t.IsNestedFamORAssem
                                && !t.IsNestedFamANDAssem;
                case ETypeTest.PUBLIC:
                    return t => t.IsVisible
                                && t.IsPublic
                                && !t.IsNotPublic
                                && !t.IsNested
                                && !t.IsNestedPublic
                                && !t.IsNestedFamily
                                && !t.IsNestedPrivate
                                && !t.IsNestedAssembly
                                && !t.IsNestedFamORAssem
                                && !t.IsNestedFamANDAssem;
                case ETypeTest.PRIVATE:
                    return t => !t.IsVisible
                                && !t.IsPublic
                                && !t.IsNotPublic
                                && t.IsNested
                                && !t.IsNestedPublic
                                && !t.IsNestedFamily
                                && t.IsNestedPrivate
                                && !t.IsNestedAssembly
                                && !t.IsNestedFamORAssem
                                && !t.IsNestedFamANDAssem;
                case ETypeTest.PROTECTED:
                    return t => !t.IsVisible
                                && !t.IsPublic
                                && !t.IsNotPublic
                                && t.IsNested
                                && !t.IsNestedPublic
                                && t.IsNestedFamily
                                && !t.IsNestedPrivate
                                && !t.IsNestedAssembly
                                && !t.IsNestedFamORAssem
                                && !t.IsNestedFamANDAssem;
                case ETypeTest.STRUCT:
                    return t => t.IsValueType;
                case ETypeTest.CLASS:
                    return t => t.IsClass;
            }

            return t => false;
        }
        /// <summary>
        /// Type of element you want to filter
        /// </summary>
        public enum ETypeTest
        {
            /// <summary>
            /// filter abtract classes
            /// </summary>
            ABSTRACT = 0,

            /// <summary>
            /// filter sealed classes
            /// </summary>
            SEALED = 1,

            /// <summary>
            /// filter generic classes
            /// </summary>
            GENERIC = 2,

            /// <summary>
            /// filter anonymous classes
            /// </summary>
            ANONYMOUS = 3,

            /// <summary>
            /// filter all value following Tobias conventions.
            /// This means, value types, strings, struct (starting with S) and enums (starting with E)
            /// </summary>
            TOBIAS_VALUE_TYPE = 4,

            /// <summary>
            /// interface
            /// </summary>
            INTERFACE = 5,

            /// <summary>
            /// enum
            /// </summary>
            ENUM = 6,

            /// <summary>
            /// static
            /// </summary>
            STATIC = 7,

            /// <summary>
            /// internal
            /// </summary>
            INTERNAL = 8,

            /// <summary>
            /// public
            /// </summary>
            PUBLIC = 9,

            /// <summary>
            /// private
            /// </summary>
            PRIVATE = 10,

            /// <summary>
            /// protected
            /// </summary>
            PROTECTED = 11,

            /// <summary>
            /// struct
            /// </summary>
            STRUCT = 12,

            /// <summary>
            /// class
            /// </summary>
            CLASS = 13,
        }
    }
}
