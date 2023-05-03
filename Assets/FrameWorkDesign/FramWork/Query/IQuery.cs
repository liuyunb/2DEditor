using System.Collections;
using System.Collections.Generic;
using FrameWorkDesign;
using UnityEngine;

namespace FrameWorkDesign
{
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModle, ICanGetSystem, ICanGetUtility
    {
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do()
        {
            return OnDo();
        }

        protected abstract T OnDo();

        private IArchitecture _mArchitecture;
        
        public IArchitecture GetArchitecture()
        {
            return _mArchitecture;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            _mArchitecture = architecture;
        }
    }
}
