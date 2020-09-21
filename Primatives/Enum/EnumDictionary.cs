using System;
using System.Collections.Generic;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// A Simple Dictionary where every member of an Enum 
    /// has an associated values that can be get; set;
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class EnumDicitonary<TEnum, TValue>
    {
        private Dictionary<TEnum, TValue> _values = new Dictionary<TEnum, TValue>();

        #region Public API
        /// <summary>
        /// Get or set an ability score
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TEnum key]
        {
            get => _values[key];
            set => _values[key] = value;
        }

        /// <summary>
        /// Set all keys to base value
        /// </summary>
        /// <param name="values"></param>
        public EnumDicitonary(TValue defaultValue = default)
        {
            TEnum[] scoreKeys = UTEnum.GetEnumAsArray<TEnum>();

            foreach (TEnum ability in scoreKeys)
            {
                _values[ability] = defaultValue;
            }
        }

        /// <summary>
        /// Construct EnumDictionary using Dictionary of preset values.
        /// This Dictionary is not copied, it is used to lookup values. Any values that are not present
        /// will be set to baseValue.
        /// </summary>
        /// <param name="values">scores to give to the set</param>
        /// <param name="baseValue">bases score of abilities not included in scores</param>
        public EnumDicitonary(Dictionary<TEnum, TValue> values, TValue defaultValue)
        {
            TEnum[] scoreKeys = UTEnum.GetEnumAsArray<TEnum>();

            if (values == null || values.Count == 0)
            {
                foreach (TEnum ability in scoreKeys)
                {
                    _values[ability] = defaultValue;
                }
            }

            foreach (TEnum ability in scoreKeys)
            {
                if (values.TryGetValue(ability, out TValue score))
                {
                    _values[ability] = score;
                }
                else
                {
                    _values[ability] = defaultValue;
                }
            }
        }
        #endregion
    }
}
