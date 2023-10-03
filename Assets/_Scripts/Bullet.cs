using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] CircleCollider2D _circleCollider;
    [SerializeField] ObVoidEvent _hitShake;
    [SerializeField] LayerMask _splodeMask;
    ObBullet _bullet;
    int _damage;
    Vector2 _lastPosition;
    static Collider2D[] _box = new Collider2D[64];
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
    public bool Setup(ObBullet bullet, Vector2 direction, Vector2 position, int layer)
    {
        if (position.x == float.NaN || position.y == float.NaN)
        {
            BackToPool();
            return false;
        }
        _bullet = bullet;
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
        return true;
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
        Vector2 hitPos = collision.ClosestPoint(_lastPosition);
        if (collision.gameObject.TryGetComponent(out IHealth health))
        {
            AudioManager.Instance.PlaySound(_bullet.hitSound, 1f);
            if (_bullet.useSleep)
            {
                GameManager.Instance.SetSleep();
            }
            if (health.TakeDamage(_damage, _rigidbody.velocity))
            {
                _hitShake.Message?.Invoke();
                if (_bullet.useSplode)
                {
                    Splode(collision.transform.position);
                }
            }
        }
        ParticleManager.Instance.SpawnParticle(_bullet.hitParticle, hitPos);
        BackToPool();

    }
    void Splode(Vector2 position)
    {
        if (Random.Range(0f, 1f) < _bullet.splodeChance)
        {
            AudioManager.Instance.PlaySound(AudioManager.SFXType.Explosion, 1f);
            position += Random.insideUnitCircle.normalized * _bullet.splodeDistance;
            AudioManager.Instance.PlaySound(AudioManager.SFXType.Explosion, 1f);
            ParticleManager.Instance.SpawnParticle(ParticleManager.Shot.Splode, position, _bullet.splodeSize);
            if (_bullet.splodeSmoke)
            {
                ParticleManager.Instance.SpawnParticle(ParticleManager.Shot.Smoke, position);
            }
            int count = Physics2D.OverlapCircleNonAlloc(position, _bullet.splodeSize, _box, _splodeMask);
            for (int i = 0; i < count; i++)
            {
                if (_box[i].gameObject.TryGetComponent(out IHealth splodetarget))
                {
                    if (splodetarget.TakeDamage(_damage, _rigidbody.velocity))
                    {
                        Splode(_box[i].transform.position);
                    }
                }


            }
        }
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