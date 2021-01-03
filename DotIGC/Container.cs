namespace DotIGC
{
    using System;
    using System.Collections.Generic;
    
    public class Container<TKey, TValue>
    {
        Dictionary<TKey, TValue> map = new Dictionary<TKey, TValue>();

        public TValue Resolve(TKey key)
        {
            TValue value;
            if (this.map.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }
        
        public IValueBinder<TValue> Bind(TKey key)
        {
            return new ValueBinder<TKey, TValue>(key, RegisterType , map.Add);
        }

        void RegisterType(TKey key, Type type)
        {
            map.Add(key, (TValue)Activator.CreateInstance(type));
        }
    }

    public interface IValueBinder<TValue>
    {
        void To<T>() where T : TValue;
        void ToConstant<T>(T constsant) where T : TValue;
    }

    internal class ValueBinder<TKey, TValue> : IValueBinder<TValue>
    {
        TKey key;
        Action<TKey, Type> to;
        Action<TKey, TValue> toConstant;

        public ValueBinder(TKey key, Action<TKey, Type> to, Action<TKey, TValue> toConstant)
        {
            this.key = key;
            this.to = to;
            this.toConstant = toConstant;
        }

        public void To<T>() where T : TValue
        {
            to(this.key, typeof(T));
        }

        public void ToConstant<T>(T constant) where T : TValue
        {
            toConstant(this.key, constant);
        }
    }
}
