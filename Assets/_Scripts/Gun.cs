using System.Collections;
using System.Collections.Generic;
using TarodevController;
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
    [SerializeField] Vector2 _offset;
    [SerializeField] UnityEvent _onShoot;
    [SerializeField] CoyotePlatformerController2D _playerController;
    [SerializeField] bool _usingStrafe;
    float _fireTime;
    int _realBulletLayer;
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
            offset.x = offset.x * Mathf.Sign(transform.lossyScale.x);
            float direction = transform.lossyScale.x > 0 ? 0 : Mathf.PI;
            direction += Random.Range(-_bullet.shake, _bullet.shake);
            Bullet bullet = Pools.Instance.Bullets.GetFrom();
            Vector2 aim = UnitDirection(direction);
            Vector2 spawnPosition = (Vector2)transform.position + offset;
            ParticleManager.Instance.SpawnParticle(_bullet.shotParticle, spawnPosition);
            AudioManager.Instance.PlaySound(_bullet.shotSound, 1f);
            bullet.Setup(_bullet, aim, spawnPosition, _realBulletLayer);
        }
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
