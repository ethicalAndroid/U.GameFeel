using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public void Setup(ObBullet bullet)
    {
        _fireTime = 0f;
        _bullet = bullet;
    }
    [SerializeField] ObBullet _bullet;

    [SerializeField] LayerMask _bulletLayer;
    [SerializeField] Vector2 _offset, _particleOffset;
    [SerializeField] UnityEvent _onShoot;
    [SerializeField] CoyotePlatformerController2D _playerController;
    [SerializeField] bool _usingStrafe;
    [SerializeField] bool _usingParticle;
    [SerializeField] Vector2 _particleMin, _particleMax;
    [SerializeField] Vector2 _knockback;
    [SerializeField] UnityEvent<Vector2> _onKnockback;
    [SerializeField] ObVoidEvent _gunScreenShake;
    float _fireTime;
    int _realBulletLayer;
    Vector2 _lastPosition;
    private void Awake()
    {
        _realBulletLayer = _bulletLayer.OneLayer();
    }
    private void Update()
    {
        bool isFiring = Input.GetKey(KeyCode.K);
        if (_usingStrafe)
        {
            _playerController.SetStrafe(isFiring);
        }
        float dt = Time.deltaTime;
        _fireTime -= dt;
        if (_fireTime < 0 && isFiring)
        {
            _onShoot?.Invoke();
            _fireTime = _bullet.reload;
            Vector2 offset = _offset;
            Vector2 particleOffset = _particleOffset;
            offset.x = offset.x * Mathf.Sign(transform.lossyScale.x);
            particleOffset.x = particleOffset.x * Mathf.Sign(transform.lossyScale.x);
            Vector2 spawnPosition = (Vector2)transform.position + offset;
            Vector2 particlePosition = (Vector2)transform.position + particleOffset;
            ParticleManager.Instance.SpawnParticle(_bullet.shotParticle, particlePosition);
            AudioManager.Instance.PlaySound(_bullet.shotSound, 1f);
            if (_usingParticle)
            {
                ShootParticle(transform.lossyScale.x < 0f);
            }
            if (spawnPosition.x == float.NaN || spawnPosition.y == float.NaN)
            {
                return;
            } 
            for (int i = 0; i < _bullet.amount; i++)
            {
                float angle = (1f - _bullet.amount) + (i * 2f);
                angle *= _bullet.spread;
                float direction = transform.lossyScale.x > 0 ? 0 : Mathf.PI;
                direction += Random.Range(-_bullet.shake, _bullet.shake) + angle;
                Bullet bullet = Pools.Instance.Bullets.GetFrom();
                Vector2 aim = UnitDirection(direction);
                bullet.Setup(_bullet, aim, spawnPosition, _realBulletLayer);
                // Particles
            }
            _gunScreenShake.Message?.Invoke();
            // Knockback
            Vector2 knockback = _knockback;
            if (transform.lossyScale.x < 0)
            {
                knockback.x = -knockback.x;
            }
            _onKnockback?.Invoke(knockback);

        }
        _lastPosition = transform.position;
    }
    private void ShootParticle(bool flip)
    {
        Vector2 particleVelocity = new Vector2(Random.Range(_particleMin.x, _particleMax.x),
            Random.Range(_particleMin.y, _particleMax.y));
        if (flip)
        {
            particleVelocity.x = -particleVelocity.x;
        }
        ParticleManager.Instance.SpawnParticle(ParticleManager.Shot.Casing, transform.position, particleVelocity, new Vector3(0f, 0f, Random.Range(0, 360f)));

    }
    private void OnDrawGizmosSelected()
    {
        float direction = Mathf.Sign(transform.lossyScale.x);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_offset * direction);
    }
    public Vector2 UnitDirection(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

}
