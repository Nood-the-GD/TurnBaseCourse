using System;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public class UnitManager : MonoBehaviorInstance<UnitManager>
    {
        private List<Unit> _unitList;
        private List<Unit> _friendUnitList;
        private List<Unit> _enemyUnitList;

        #region Unit functions
        protected override void ChildAwake()
        {
            _unitList = new List<Unit>();
            _friendUnitList = new List<Unit>();
            _enemyUnitList = new List<Unit>();
            Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawnedHandler;
            Unit.OnAnyUnitDestroyed += Unit_OnAnyUnitDestroyedHandler;
        }
        private void Start()
        {
        }
        private void OnDestroy()
        {
            Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawnedHandler;
            Unit.OnAnyUnitDestroyed -= Unit_OnAnyUnitDestroyedHandler;
        }
        #endregion

        #region Event functions
        private void Unit_OnAnyUnitSpawnedHandler(object sender, EventArgs eventArgs)
        {
            Unit unit = sender as Unit;
            if(unit.IsEnemy())
            {
                _enemyUnitList.Add(unit);
            }
            else
            {
                _friendUnitList.Add(unit);
            }
            _unitList.Add(unit);
        }
        private void Unit_OnAnyUnitDestroyedHandler(object sender, EventArgs eventArgs)
        {
            Unit unit = sender as Unit;
            if(unit.IsEnemy())
            {
                _enemyUnitList.Remove(unit);
            }
            else
            {
                _friendUnitList.Remove(unit);
            }
            _unitList.Remove(unit);
        }
        #endregion

        #region Get
        public List<Unit> GetUnitList()
        {
            return _unitList;
        }
        public List<Unit> GetFriendlyUnitList()
        {
            return _friendUnitList;
        }
        public List<Unit> GetEnemyUnitList()
        {
            return _enemyUnitList;
        }
        #endregion 
    }
}
