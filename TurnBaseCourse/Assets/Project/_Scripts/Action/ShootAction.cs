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
        public static event EventHandler<OnShootEventArgs> OnAnyShoot;
        public event EventHandler<OnShootEventArgs> OnShoot;
        public class OnShootEventArgs:EventArgs
        {
            public Unit targetUnit;
            public Unit shootingUnit;
        }
        #endregion

        #region Variables
        [SerializeField] private int _damage = 40;
        [SerializeField] private int _maxShootDistance = 7;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private BulletScript _bulletPref;
        [SerializeField] private Transform _shootPoint;
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canShootBullet;
        private float _rotateSpeed = 10f;
        private List<Unit> _targetUnitList;
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
            OnAnyShoot?.Invoke(this, new OnShootEventArgs
            {
                targetUnit = _targetUnit,
                shootingUnit = _unit
            });
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                targetUnit = _targetUnit,
                shootingUnit = _unit
            });

            BulletScript bulletScript = Instantiate<BulletScript>(_bulletPref, _shootPoint.position, Quaternion.identity);

            Vector3 shootPos = _targetUnit.GetWorldPosition();
            shootPos.y = bulletScript.transform.position.y;

            bulletScript.Setup(shootPos);
            Debug.Log(_shootPoint.position);

            _targetUnit.Damage(_damage, shootPos);
        }
        private void RotateHandler(Vector3 moveDirection)
        {
            this.transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
        }
        public Unit GetTargetUnit()
        {
            return _targetUnit;
        }
        public int GetRange()
        {
            return _maxShootDistance;
        }
        /// <summary>
        /// Get target count when stand at this gridPosition
        /// </summary>
        /// <param name="gridPosition">the gridPosition AI will stand</param>
        /// <returns></returns>
        public int GetTargetCountWhenStandAtGridPosition(GridPosition gridPosition)
        {
            return GetValidGridPositionList(gridPosition).Count;
        }
        /// <summary>
        /// Return valid grid position list that can shoot from this unit grid position
        /// </summary>
        /// <param name="unitGridPosition"></param>
        /// <returns></returns>
        public List<GridPosition> GetValidGridPositionList(GridPosition unitGridPosition)
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
            {
                for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
                {
                    GridPosition offSetGridPosition = new GridPosition(x, z);
                    GridPosition testingGridPosition = unitGridPosition + offSetGridPosition;

                    if(!LevelGrid.Instance.IsValidGrid(testingGridPosition))
                    {
                        // Grid is not valid in this current level
                        continue;
                    }
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if(testDistance > _maxShootDistance)
                    {
                        continue;
                    }

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testingGridPosition))
                    {
                        // Grid is empty, no unit
                        continue;
                    }

                    Unit unit = LevelGrid.Instance.GetUnitAtGridPosition(testingGridPosition);

                    if(unit.IsEnemy() == _unit.IsEnemy())
                    {
                        // Both Unit on same "team"
                        continue;
                    }

                    Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                    Vector3 shootDir = (testingGridPosition.GetWorldPosition() - unitWorldPosition).normalized;
                    float unitShoulderHeight = 1.7f;
                    if(Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir, Vector3.Distance(unitWorldPosition, testingGridPosition.GetWorldPosition()), _obstacleLayerMask))
                    {
                        // Block by obstacle
                        continue;
                    }

                    validGridPositionList.Add(testingGridPosition);
                }
            }
            return validGridPositionList;
        }
        #endregion

        #region Override functions
        public override string GetActionName()
        {
            return "Shoot";
        }
        public override List<GridPosition> GetValidGridPositionList()
        {
            GridPosition unitGridPosition = _unit.GetCurrentGridPosition();
            return GetValidGridPositionList(unitGridPosition);            
        }
        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            // Shoot

            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            _state = State.Aiming;
            float aimingStateTime = 1f;
            _stateTimer = aimingStateTime;
            _canShootBullet = true;

            ActionStart(onActionComplete);
        }
        /// <summary>
        /// Create a EnemyAIAction with gridPosition is position of target unit
        /// </summary>
        /// <param name="gridPosition">position of target unit</param>
        /// <returns></returns>
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            Unit unit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 200 - unit.GetCurrentHealth()
            };
        }
        #endregion
    }
}
