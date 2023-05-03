#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FrameWorkDesign
{
    public interface IStorage : IUtility
    {
        void SaveInt(string key, int value);
        int LoadInt(string key, int defaultValue);
        
    }

    public class PlayerPrefers : IStorage
    {
        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key,value);
        }

        public int LoadInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key,defaultValue);
        }
    }
    
    public class EditorPrefers : IStorage
    {
        public void SaveInt(string key, int value)
        {
#if UNITY_EDITOR
            EditorPrefs.SetInt(key,value);
#endif
        }

        public int LoadInt(string key, int defaultValue)
        {
#if UNITY_EDITOR
            return EditorPrefs.GetInt(key,defaultValue);
#else 
            return 0;
#endif
            
        }
    }
}