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
        private static T s_instance;
        public static T Instance
        {
            get
            {
                if (s_instance == null) Debug.Log($"Can't find {typeof(T)}");
                return s_instance;
            }
        }

        protected void Awake()
        {
            if (s_instance != null)
            {
                Debug.LogError($"Exist 2 {typeof(T)} in the scene {this.gameObject.name} and {s_instance.gameObject.name}");
                Destroy(s_instance.gameObject);
            }
            s_instance = this as T;
            ChildAwake();
        }

        protected virtual void ChildAwake()
        {}
    }
}

