using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] CircleCollider2D _circleCollider;
    int _damage;
    Vector2 _lastPosition;
    ParticleManager.Shot _onHitParticle;
    AudioManager.SFXType _onHitSound;
    private void FixedUpdate()
    {
        _lastPosition = _rigidbody.position;
    }
    private void Start()
    {
        Pools.Instance.OnRecall += BackToPool;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Pools.Instance.OnRecall -= BackToPool;
    }
    public void Setup(ObBullet bullet, Vector2 direction, Vector2 position, int layer)
    {
        // Activate!
        gameObject.SetActive(true);
        // Set position
        _rigidbody.position = position;
        transform.position = position;
        // Set collisions
        _circleCollider.radius = bullet.size;
        gameObject.layer = layer;
        // Set sprite
        _renderer.sprite = bullet.sprite;
        _renderer.transform.localScale = new Vector3(bullet.spriteSize, bullet.spriteSize);
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _renderer.transform.eulerAngles = new Vector3(0, 0, rotation);
        // Set velocity
        _rigidbody.velocity = direction * bullet.velocity;
        // Set bullet attack
        _damage = bullet.damage;
        _onHitParticle = bullet.hitParticle;
        _onHitSound = bullet.hitSound;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.TryGetComponent(out IHealth health))
        {
            health.TakeDamage(_damage, _rigidbody.velocity);
        }
        ParticleManager.Instance.SpawnParticle(_onHitParticle, collision.GetContact(0).point);
        BackToPool();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Collide(collision);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collide(collision);
    }
    void Collide(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IHealth health))
        {
            health.TakeDamage(_damage, _rigidbody.velocity);
            AudioManager.Instance.PlaySound(_onHitSound, 1f);
        }
        ParticleManager.Instance.SpawnParticle(_onHitParticle, collision.ClosestPoint(_lastPosition));
        BackToPool();
        
    }
    void BackToPool()
    {
        Pools.TryRelease(Pools.Instance.Bullets, this);
    }
}
public interface IHealth
{
    public bool TakeDamage(int damage, Vector2 direction);
}