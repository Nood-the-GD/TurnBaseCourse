using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace NOOD
{
    public class MonoBehaviorInstance <T> : AbstractMonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] protected bool _dontDestroyOnLoad;

        private static T s_instance;
        public static T Instance
        {
            get
            {
                if (s_instance == null) TryGetInstance();
                return s_instance;
            }
        }

        protected void Awake()
        {
            if (s_instance != null && s_instance.gameObject != this.gameObject)
            {
                Debug.LogError($"Exist 2 {typeof(T)} in the scene {this.gameObject.name} and {s_instance.gameObject.name}");
                Destroy(this.gameObject);
            }
            else
            {
                TryGetInstance();
            }
            if(_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(s_instance.gameObject);
            }
            ChildAwake();
        }

        private static T TryGetInstance()
        {
            s_instance = GameObject.FindObjectOfType<T>();
            return s_instance;
        }

        protected virtual void ChildAwake()
        {}
    }
}

