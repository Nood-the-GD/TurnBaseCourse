using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpinAction : BaseAction
    {
        private float _totalSpinAmount;
        
        private void Update()
        {
            if(_isActive)
            {
                float spinAmount = 360f * Time.deltaTime;
                _totalSpinAmount += spinAmount;
                if(_totalSpinAmount >= 360)
                {
                    _isActive = false;
                    OnActionComplete?.Invoke();
                }
                transform.eulerAngles += new Vector3(0, spinAmount, 0);
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
        {
            OnActionComplete += onSpinComplete;
            _isActive = true;
            _totalSpinAmount = 0;
        }

        public override List<GridPosition> GetValidGridPositionList()
        {
            throw new NotImplementedException();
        }

        public override string GetActionName()
        {
            return "Spin";
        }
    }
}
