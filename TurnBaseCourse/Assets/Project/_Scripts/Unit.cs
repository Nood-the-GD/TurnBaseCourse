using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _rotateSpeed = 10;

        private GridPosition _gridPosition;
        private MoveAction _moveAction;

        #region Unity Functions
        private void Awake()
        {
            _moveAction = this.GetComponent<MoveAction>();
        }
        private void Start()
        {
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
                LevelGrid.Instance.UnitMoveGridPosition(this, _gridPosition, newGridPosition);
                _gridPosition = newGridPosition;
            }
        }
        #endregion

        public MoveAction GetMoveAction()
        {
            return this._moveAction;
        }

        public GridPosition GetCurrentGridPosition()
        {
            return _gridPosition;
        }
    }
}