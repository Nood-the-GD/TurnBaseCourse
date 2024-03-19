using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _debugText;
        private object _gridObject;

        public virtual void SetGridObject(object gridObject)
        {
            this._gridObject = gridObject;
        }

        protected virtual void Update()
        {
            _debugText.text = _gridObject.ToString();
        }
    }
}