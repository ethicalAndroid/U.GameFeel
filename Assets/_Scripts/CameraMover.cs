using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] bool _lerping;
    [SerializeField, Range(0, 1f)] float _lerpAmount;
    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }
        Vector2 nextPosition = _lerping ? Vector2.Lerp(_target.position, transform.position, _lerpAmount) : _target.position;        
        transform.position = nextPosition;
        
    }
}
