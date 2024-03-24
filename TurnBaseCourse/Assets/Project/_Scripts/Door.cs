using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _leftDoor, _rightDoor;
        private Action _onInteractComplete;
        private bool _isOpen = false;
        private float _timer;
        private bool _isActive;

        void Start()
        {
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
            LevelGrid.Instance.AddInteractableObjectAtThisGrid(gridPosition, this);
        }

        private void Update()
        {
            if (!_isActive) return;
            _timer -= Time.deltaTime;
            
            if(_timer <= 0)
            {
                _onInteractComplete?.Invoke();
                _isActive = false;  
            }
        }

        public void Interact(Action onInteractComplete)
        {
            _timer -= Time.deltaTime;

            _isOpen = !_isOpen;
            if (_isOpen)
                OpenDoor();
            else
                CloseDoor();
            this._onInteractComplete = onInteractComplete;
            _isActive = true;
        }

        private void OpenDoor()
        {
            _timer = 0.5f;
            _leftDoor.DOScaleX(0.4f, 0.5f);
            _rightDoor.DOScaleX(0.4f, 0.5f);
        }
        private void CloseDoor()
        {
            _timer = 0.5f;
            _leftDoor.DOScaleX(1f, 0.5f);
            _rightDoor.DOScaleX(1f, 0.5f);
        }
    }
}
