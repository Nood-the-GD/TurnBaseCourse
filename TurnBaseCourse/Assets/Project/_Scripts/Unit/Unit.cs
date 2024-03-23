using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using static Game.HealthSystem;

namespace Game
{
    public class Unit : MonoBehaviour
    {
        #region Events
        public static event EventHandler OnAnyActionPointsChanged;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDestroyed;
        #endregion

        #region Variable
        private const int ACTION_POINT_MAX = 2;

        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _rotateSpeed = 10;
        [SerializeField] private bool _isEnemy;

        private HealthSystem _healthSystem;

        private GridPosition _gridPosition;
        private BaseAction[] _baseActionArray;
        private int _actionPoint = 2;
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _baseActionArray = this.GetComponents<BaseAction>();
            _healthSystem = this.GetComponent<HealthSystem>();
        }
        private void OnEnable()
        {
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChangeHandler;
            _healthSystem.OnDead += HealthSystem_OnDeadHandler;
        }
        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
            GetAction<MoveAction>().SetMoveProperty(_moveSpeed, _rotateSpeed);
            OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        }
        private void Update()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
            if (newGridPosition != _gridPosition)
            {
                // Unit change grid position
                GridPosition oldGridPosition = _gridPosition;
                _gridPosition = newGridPosition;

                LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
            }
        }
        private void OnDisable()
        {
            NoodyCustomCode.UnSubscribeAllEvent<TurnSystem>(this);
            _healthSystem.OnDead -= HealthSystem_OnDeadHandler;
        }
        private void OnDestroy()
        {
            
        }
        #endregion

        #region Actions
        public T GetAction<T>() where T : BaseAction
        {
            foreach(BaseAction baseAction in _baseActionArray)
            {
                if(baseAction is T)
                {
                    return (T)baseAction;
                }
            }
            return null;
        }
        #endregion

        #region Support
        public bool IsEnemy()
        {
            return _isEnemy;
        }
        public GridPosition GetCurrentGridPosition()
        {
            return _gridPosition;
        }
        public BaseAction[] GetBaseActionArray()
        {
            return _baseActionArray;
        }
        public int GetActionPoint()
        {
            return _actionPoint;
        }
        public void Damage(int healthAmount, Vector3 damagePosition)
        {
            _healthSystem.Damage(healthAmount, damagePosition);
        }
        public Vector3 GetWorldPosition()
        {
            return this.transform.position;
        }
        public int GetCurrentHealth()
        {
            return _healthSystem.GetCurrentHealth();
        }
        #endregion

        #region Do Action
        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (CanSpendActionPointToTakeAction(baseAction))
            {
                SpendActionPoint(baseAction.GetActionPointCost());
                return true;
            }
            else return false;
        }
        public bool CanSpendActionPointToTakeAction(BaseAction baseAction)
        {
            return _actionPoint >= baseAction.GetActionPointCost();
        }
        private void SpendActionPoint(int amount)
        {
            _actionPoint -= amount;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Event functions
        private void TurnSystem_OnTurnChangeHandler(object sender, EventArgs eventArgs)
        {
            if(IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() || !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
            {
                _actionPoint = ACTION_POINT_MAX;            
                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private void HealthSystem_OnDeadHandler(object sender, OnDeadEventArgs eventArgs)
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(GetCurrentGridPosition(), this);
            Destroy(this.gameObject);
            OnAnyUnitDestroyed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}