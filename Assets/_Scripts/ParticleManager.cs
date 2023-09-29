using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] _systems;
    [SerializeField] Shot _lookup;
    public enum Shot
    {
        None = 999,
        Flash = 0,
        EnemyDead = 1,
        Casing = 2,
        Splode = 3,
        Smoke = 4,
    }
    public static ParticleManager Instance;
    private void Awake()
    {
        if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
        Instance = this;
    }
    public void SpawnParticle(Shot shot, Vector2 position)
    {
        if (shot != Shot.None)
        {
            ParticleSystem.EmitParams emitParams = new();
            emitParams.position = position;
            _systems[(int)shot].Emit(emitParams, 1);
        }
    }
    public void SpawnParticle(Shot shot, Vector2 position, float size)
    {
        if (shot != Shot.None)
        {
            ParticleSystem.EmitParams emitParams = new();
            emitParams.position = position;
            emitParams.startSize = size;
            _systems[(int)shot].Emit(emitParams, 1);
        }
    }
    public void SpawnParticle(Shot shot, Vector2 position, Vector2 velocity, Vector3 rotation)
    {
        if (shot != Shot.None)
        {
            ParticleSystem.EmitParams emitParams = new();
            emitParams.position = position;
            emitParams.velocity = velocity;
            emitParams.rotation3D = rotation;
            _systems[(int)shot].Emit(emitParams, 1);
        }
    }

}
