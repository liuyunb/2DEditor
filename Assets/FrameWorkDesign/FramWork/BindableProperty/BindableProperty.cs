using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWorkDesign
{
    public class BindableProperty<T>
    {
        private T _mCount;
        public T Value
        {
            get => _mCount;
            set
            {
                if (!value.Equals(_mCount))
                {
                    _mCount = value;
                    mOnValueChanged?.Invoke(value);
                }
            }
        }

        // public Action<T> OnCountChanged;

        private Action<T> mOnValueChanged;

        public IUnRegister RegisterOnValueChanged(Action<T> e)
        {
            mOnValueChanged += e;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = mOnValueChanged
            };
        }

        public void UnRegisterOnValueChanged(Action<T> e)
        {
            mOnValueChanged -= e;
        }
    }

    public class BindablePropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }
        public Action<T> OnValueChanged;
        public void UnRegister()
        {
            if(BindableProperty != null)
                BindableProperty.UnRegisterOnValueChanged(OnValueChanged);
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
}

