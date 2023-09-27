using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] _systems;
    public enum Shot
    {
        None = 999,
        Flash = 0,
        Kaboom = 1,
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

}
