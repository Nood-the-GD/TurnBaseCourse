using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ShootAction : BaseAction
    {
        private enum State
        {
            Aiming,
            Shooting,
            CoolOff,
        }

        #region Events
        public event EventHandler<OnShootEventArgs> OnShoot;
        public class OnShootEventArgs:EventArgs
        {
            public Unit targetUnit;
            public Unit shootingUnit;
        }
        #endregion

        #region Variables
        [SerializeField] private int _damage = 40;
        private State _state;
        private int _maxShootDistance = 7;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canShootBullet;
        private float _rotateSpeed = 10f;
        #endregion

        #region Unity functions
        private void Update()
        {
            if (!_isActive) return;

            _stateTimer -= Time.deltaTime;

            switch (_state)
            {
                case State.Aiming:
                    Vector3 direction = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                    RotateHandler(direction);
                    break;
                case State.Shooting:
                    if(_canShootBullet)
                    {
                        _canShootBullet = false;
                        Shoot();
                    }
                    break;
                case State.CoolOff:
                    break;
            }

            if (_stateTimer <= 0f)
            {
                NextState();
            }
        }
        #endregion

        #region State
        private void NextState()
        {
            switch (_state)
            {
                case State.Aiming:
                    _state = State.Shooting;
                    float shootingStateTimer = .1f;
                    _stateTimer = shootingStateTimer;
                    break;
                case State.Shooting:
                    _state = State.CoolOff;
                    float coolOffStateTime = 0.5f;
                    _stateTimer = coolOffStateTime;
                    break;
                case State.CoolOff:
                    ActionComplete();
                    break;
            }
        }
        #endregion

        #region Support
        private void Shoot()
        {
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                targetUnit = _targetUnit,
                shootingUnit = _unit
            });
            _targetUnit.Damage(_damage);
        }
        private void RotateHandler(Vector3 moveDirection)
        {
            this.transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
        }
        #endregion

        #region Override functions
        public override string GetActionName()
        {
            return "Shoot";
        }
        public override List<GridPosition> GetValidGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = _unit.GetCurrentGridPosition();

            for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
            {
                for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
                {
                    GridPosition offSetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                    if(!LevelGrid.Instance.IsValidGrid(testGridPosition))
                    {
                        // Grid is not valid in this current level
                        continue;
                    }
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if(testDistance > _maxShootDistance)
                    {
                        continue;
                    }

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        // Grid is empty, no unit
                        continue;
                    }

                    Unit unit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    if(unit.IsEnemy() == _unit.IsEnemy())
                    {
                        // Both Unit on same "team"
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }
            return validGridPositionList;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            // Shoot
            ActionStart(onActionComplete);

            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            _state = State.Aiming;
            float aimingStateTime = 1f;
            _stateTimer = aimingStateTime;
            _canShootBullet = true;
        }
        #endregion
    }
}
