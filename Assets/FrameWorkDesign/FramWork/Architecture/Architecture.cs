using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWorkDesign
{
    public interface IArchitecture
    {
        void RegisterSystem<T>(T instance) where T : class, ISystem;
        
        void RegisterModle<T>(T instance) where T : class, IModel;
        
        void RegisterUtility<T>(T instance) where T : class, IUtility;

        T GetSystem<T>() where T : class, ISystem;
        
        T GetModle<T>() where T : class, IModel;
        
        T GetUtility<T>() where T : class, IUtility;

        void SendCommand<T>() where T : ICommand, new();

        void SendCommand<T>(T command) where T : ICommand;

        TResult SendQuery<TResult>(IQuery<TResult> query);

        void SendEvent<T>() where T : new();

        void SendEvent<T>(T e);

        IUnRegister RegisterEvent<T>(Action<T> e);

        void UnRegister<T>(Action<T> e);
    }
    
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private bool mInited = false;

        private List<ISystem> mSystems = new List<ISystem>();
        
        private List<IModel> mModles = new List<IModel>();

        private static T _instance = null;

        private static IOC _iocContainner = new IOC();

        public static Action<T> OnRegisterPatch = architecture => { };
        
        public static IArchitecture Instance{
            get
            {
                if(_instance == null)
                    MakeSureArchitecture();

                return _instance;
            }
        }

        private static void MakeSureArchitecture()
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Init();
                    
                OnRegisterPatch?.Invoke(_instance);
                
                foreach (var modle in _instance.mModles)
                {
                    modle.Init();
                }
                
                foreach (var system in _instance.mSystems)
                {
                    system.Init();
                }
                
                _instance.mModles.Clear();
                _instance.mSystems.Clear();
                
                _instance.mInited = true;
            }
        }

        protected abstract void Init();

        public static TK Get<TK>() where TK : class
        {
            MakeSureArchitecture();

            return _iocContainner.Get<TK>();
        }

        public void Register<TK>(TK instance) where TK : class
        {
            MakeSureArchitecture();
            _iocContainner.Register(instance);
        }

        public void RegisterSystem<T1>(T1 instance) where T1 : class, ISystem
        {
            instance.SetArchitecture(this); 
            _iocContainner.Register(instance);
            if(!mInited)
                mSystems.Add(instance);
            else
                instance.Init();
        }

        public void RegisterModle<T1>(T1 instance) where T1 : class, IModel
        {
            instance.SetArchitecture(this);
            _iocContainner.Register(instance);
            if(!mInited)
                mModles.Add(instance);
            else
                instance.Init();
        }

        public void RegisterUtility<T1>(T1 instance) where T1 : class, IUtility
        {
            _iocContainner.Register(instance);
        }

        public T1 GetSystem<T1>() where T1 : class, ISystem
        {
            return Get<T1>();
        }

        public T1 GetModle<T1>() where T1 : class, IModel
        {
            return Get<T1>();
        }

        public T1 GetUtility<T1>() where T1 : class, IUtility
        {
            return Get<T1>();
        }

        public void SendCommand<T1>() where T1 : ICommand, new()
        {
            var command = new T1();
            command.SetArchitecture(this);
            command.Execute();
        }

        public void SendCommand<T1>(T1 command) where T1 : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        public void SendEvent<T1>() where T1 : new()
        {
            mTypeEventSystem.Send<T1>();
        }

        public void SendEvent<T1>(T1 e)
        {
            mTypeEventSystem.Send<T1>(e);
        }

        public IUnRegister RegisterEvent<T1>(Action<T1> e)
        {
            return mTypeEventSystem.Register(e);
        }

        public void UnRegister<T1>(Action<T1> e)
        {
            mTypeEventSystem.UnRegister(e);
        }
    }
}