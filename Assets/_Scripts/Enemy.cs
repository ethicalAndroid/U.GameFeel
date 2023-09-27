using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    const float RAY_LENGTH = 0.6f;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Transform _animbody;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] int _health;
    [SerializeField] float _speed;
    float _direction = -1f;
    [SerializeField] float _kickback;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] bool _usesHitFlash;
    [SerializeField] float _knockbackPower;
    Vector2 _knockback = Vector2.zero;
    FlashRenderer _flashRenderer;
    void Awake()
    {
        _flashRenderer = new(_renderer);
    }
    void Update()
    {
        _flashRenderer.Update(Time.deltaTime);
    }
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, new Vector2(_direction, 0f), RAY_LENGTH, _wallLayer);
        if (hit.collider)
        {
            _direction = -_direction;
        }
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = _direction * _speed;
        _rigidbody.velocity = velocity + _knockback;
        _animbody.localScale = new Vector3(Mathf.Sign(_direction), 1f, 1f);
        _knockback = Vector2.zero;

    }
    public bool TakeDamage(int damage, Vector2 direction)
    {
        _health -= damage;
        _knockback = direction.normalized * _knockbackPower;
        if (_usesHitFlash)
        {
            _flashRenderer.SetFlash();
        }
        if (_health <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_rigidbody.position, _rigidbody.position + new Vector2(_direction, 0f) * RAY_LENGTH);
    }
}
