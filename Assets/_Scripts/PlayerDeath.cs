using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _vcamera;
    [SerializeField] LayerMask _killLayer;
    [SerializeField] Vector2 _velocity;
    [SerializeField] Vector2 _gunVelocity;
    [SerializeField] float _gunRotation;
    [SerializeField] Rigidbody2D _rigidbody, _gun;
    int _killLayerInt;
    private void Awake()
    {
        _killLayerInt = _killLayer.OneLayer();
    }
    void Death(float direction)
    {
        AudioManager.Instance.PlaySound(AudioManager.SFXType.PlayerDeath, 1f);
        _rigidbody.gameObject.SetActive(true);
        _rigidbody.transform.SetParent(null);
        _gun.transform.SetParent(null);
        _vcamera.Follow = _rigidbody.transform;
        _rigidbody.velocity = SetDirection(direction, _velocity);
        _gun.velocity = SetDirection(direction, _gunVelocity);
        _rigidbody.transform.localScale = new Vector3(direction, 1f, 1f);
        _gun.transform.localScale = new Vector3(transform.lossyScale.x, 1f, 1f);
        _gun.AddTorque(_gunRotation * direction);
        gameObject.SetActive(false);
        GameManager.Instance.SetSlow();
    }
    private Vector2 SetDirection(float direction, Vector2 velocity)
    {
        velocity.x *= direction;
        return velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _killLayerInt)
        {
            Death(Mathf.Sign(collision.rigidbody.velocity.x));
        }
    }
}
