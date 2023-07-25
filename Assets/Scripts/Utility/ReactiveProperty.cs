using System;

namespace AnimarsCatcher
{
    public class ReactiveProperty<T> where T:IEquatable<T>
    {
        private T mValue;
        private Action<T> mOnValueChanged;

        public T Value
        {
            get => mValue;
            set
            {
                if (!mValue.Equals(value))
                {
                    mValue = value;
                    mOnValueChanged?.Invoke(mValue);
                }
            }
        }

        public ReactiveProperty(T initialValue, Action<T> onValueChanged = null)
        {
            mValue = initialValue;
            mOnValueChanged = onValueChanged;
        }
        
        public ReactiveProperty(Action<T> onValueChanged = null)
        {
            mOnValueChanged = onValueChanged;
        }

        public void Subscribe(Action<T> callback)
        {
            mOnValueChanged += callback;
        }

        public void Unsubscribe(Action<T> callback)
        {
            mOnValueChanged -= callback;
        }
    }
}