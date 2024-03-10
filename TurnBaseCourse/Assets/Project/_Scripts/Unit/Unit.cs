using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.HealthSystem;

namespace Game
{
    public class Unit : MonoBehaviour
    {
        #region Variable
        private const int ACTION_POINT_MAX = 2;

        public static event EventHandler OnAnyActionPointsChanged;

        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _rotateSpeed = 10;
        [SerializeField] private bool _isEnemy;

        private HealthSystem _healthSystem;

        private GridPosition _gridPosition;
        private MoveAction _moveAction;
        private SpinAction _spinAction;
        private BaseAction[] _baseActionArray;
        private int _actionPoint = 2;
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _baseActionArray = this.GetComponents<BaseAction>();
            _moveAction = this.GetComponent<MoveAction>();
            _spinAction = this.GetComponent<SpinAction>();
            _healthSystem = this.GetComponent<HealthSystem>();
        }
        private void Start()
        {
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChangeHandler;
            _healthSystem.OnDead += HealthSystem_OnDeadHandler;

            _gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
            _moveAction.SetMoveProperty(_moveSpeed, _rotateSpeed);
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
        private void OnDestroy()
        {
            TurnSystem.Instance.OnTurnChange -= TurnSystem_OnTurnChangeHandler;
        }
        #endregion

        #region Actions
        public MoveAction GetMoveAction()
        {
            return this._moveAction;
        }
        public SpinAction GetSpinAction()
        {
            return this._spinAction;
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
        public void Damage(int healthAmount)
        {
            _healthSystem.Damage(healthAmount);
        }
        public Vector3 GetWorldPosition()
        {
            return this.transform.position;
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
        }
        #endregion
    }
}