using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform _startPos, _endPos;
    [SerializeField] GameObject _spawn;
    [SerializeField, Min(0)] int _amount = 1;
    private void Awake()
    {
        foreach (Vector2 pos in GetPositions())
        {
            Instantiate(_spawn, pos, Quaternion.identity);
        }
    }
    private void OnDrawGizmos()
    {
        if (_startPos && _endPos)
        {
            Gizmos.DrawLine(_startPos.position, _endPos.position);
            foreach (Vector2 pos in GetPositions())
            {
                Gizmos.DrawSphere(pos, 0.5f);
            }
        }
    }
    IEnumerable<Vector2> GetPositions()
    {
        for (int i = 0; i < _amount; i++)
        {
            float distance = (float)i * (1f / (float)_amount);
            Vector2 pos = Vector2.Lerp(_startPos.position, _endPos.position, distance);
            yield return pos;
        }
    }
}
