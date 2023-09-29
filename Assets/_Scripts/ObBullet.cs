using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBullet", menuName = "Ob/Bullet")]
public class ObBullet : ScriptableObject
{
    [field: SerializeField] public float velocity { get; private set; }
    [field: SerializeField] public float reload { get; private set; }
    [field: SerializeField] public int amount { get; private set; } = 1;
    [field: SerializeField] public float spread { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float spriteSize { get; private set; }
    [field: SerializeField] public float shake { get; private set; }
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public ParticleManager.Shot shotParticle { get; private set; }
    [field: SerializeField] public ParticleManager.Shot hitParticle { get; private set; }
    [field: SerializeField] public AudioManager.SFXType shotSound { get; private set; }
    [field: SerializeField] public AudioManager.SFXType hitSound { get; private set; }
    [field: SerializeField] public bool useSleep { get; private set; }
    [field: SerializeField] public int splodeDamage { get; private set; }
    [field: SerializeField] public float splodeSize { get; private set; }
    [field: SerializeField] public float splodeDistance { get; private set; }
    [field: SerializeField, Range(0f, 1f)] public float splodeChance { get; private set; }
    [field: SerializeField] public bool splodeSmoke { get; private set; }
    [field: SerializeField] public bool useSplode { get; private set; }



}
