using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _unitAnimator;
    
    private Vector3 _targetPosition;
    private float _moveSpeed = 4;
    private float _rotateSpeed = 10;
    private float _distanceToStop = 0.1f;

    private void Awake()
    {
        _targetPosition = this.transform.position;
    }

    private void Start()
    {
        GridPositionStruct gridPositionStruct = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(gridPositionStruct, this);
    }

    private void Update()
    {
        float distance = Vector3.Distance(this.transform.position,_targetPosition);
        if (distance > _distanceToStop)
        {
            _unitAnimator.SetBool("Walk", true);
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
        }
        else
            _unitAnimator.SetBool("Walk", false);

    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
