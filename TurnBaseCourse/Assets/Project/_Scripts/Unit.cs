using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 _targetPosition;
    private float _moveSpeed = 4;
    private float _distanceToStop = 0.1f;

    private void Update()
    {
        float distance = Vector3.Distance(this.transform.position,_targetPosition);
        if (distance > _distanceToStop)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            SetTargetPosition(MouseWorld.GetPosition());
        }
    }

    private void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
