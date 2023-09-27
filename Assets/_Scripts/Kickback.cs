using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kickback : MonoBehaviour
{
    [SerializeField] float _gunKickSpring;
    [SerializeField] float _gunKickMin;
    [SerializeField] float _gunKickMax;
    [SerializeField] float _kickForce;
    float _gunKick;
    private void Update()
    {
        float dt = Time.deltaTime;
        _gunKick = Mathf.Clamp(_gunKick + dt * _gunKickSpring, _gunKickMin, _gunKickMax);
        transform.localPosition = new Vector3(_gunKick, 0f, 0f);
    }
    public void GunKickActivate()
    {
        _gunKick += _kickForce;
    }
}
