using System;
using NOOD;
using UnityEngine;

namespace Game
{
    public class PathFindingUpdater : MonoBehaviour
    {
        #region Unity functions
        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectActionChange += UnitActionSystem_OnSelectActionSelectedHandler;
        }
        private void OnDisable()
        {
            NoodyCustomCode.UnSubscribeAllEvent<UnitActionSystem>(this);
        }
        #endregion

        #region Event functions
        private void UnitActionSystem_OnSelectActionSelectedHandler(object sender, EventArgs e)
        {
            if(UnitActionSystem.Instance.GetSelectedAction() is MoveAction)
                PathFinding.Instance.UpdatePathFinding();
        }
        #endregion
    }
}
