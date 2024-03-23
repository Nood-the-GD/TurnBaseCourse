using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class PathFindingGridDebugObject : GridDebugObject
    {
        [SerializeField] private TextMeshPro _gCostText, _hCostText, _fCostText;

        private PathNode _pathNode;

        public override void SetGridObject(object gridObject)
        {
            base.SetGridObject(gridObject);
            this._pathNode = (PathNode) gridObject;
        }

        protected override void Update()
        {
            base.Update();
            _gCostText.text = _pathNode.GetGCost().ToString();
            _hCostText.text = _pathNode.GetHCost().ToString();
            _fCostText.text = _pathNode.GetFCost().ToString();
        }
    }
}
