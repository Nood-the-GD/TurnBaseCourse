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
                return s_instance;
            }
        }

        protected void Awake()
        {
            if (s_instance != null)
            {
                Debug.LogError($"Exist 2 {typeof(T)} in the scene {this.gameObject.name} and {s_instance.gameObject.name}");
            }
            s_instance = this as T;
            ChildAwake();
        }

        protected virtual void ChildAwake()
        {}
    }
}

