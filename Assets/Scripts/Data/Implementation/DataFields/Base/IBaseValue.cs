using System;

namespace Game.Data.DataFields.Base
{
    public interface IBaseValue<T> where T : struct, IComparable, IFormattable, IConvertible, IEquatable<T>
    {
        public T BaseValue { get; }
    }
}