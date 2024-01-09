using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NOOD
{
    public class MonoBehaviorInstance <T> : AbstractMonoBehaviour where T : MonoBehaviour
    {

        public static T Instance
        {
            get
            {
                if (InstanceHolder.GetInstance<T>())
                {
                    return InstanceHolder.GetInstance<T>();
                }
                else
                    Debug.LogError("Can't find Instance of " + typeof(T));
                return null;        
            }
        }

        void Awake()
        {
            InstanceHolder.AddToInstanceDic(typeof(T), this);
        }
    }

    public static class InstanceHolder
    {
        private static Dictionary<Type, object> _instanceDic = new Dictionary<Type, object>();

        public static T GetInstance<T>() where T : MonoBehaviour
        {
            if (_instanceDic.TryGetValue(typeof(T), out object instance))
            {
                return (T)instance;
            }
            else
                return null;
        }

        public static void AddToInstanceDic(Type type, object instance)
        {
            if(!_instanceDic.TryAdd(type, instance))
            {
                _instanceDic[type] = instance;
            }
        }
    }
}

