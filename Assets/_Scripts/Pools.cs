using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pools : MonoBehaviour
{
    [System.Serializable]
    struct PoolSpec<T> where T : MonoBehaviour
    {
        public T prefab;
        public int amount;
    }
    static public Pools Instance { get; private set; }

    // Each pool needs a pool spec
    [SerializeField] PoolSpec<Bullet> _bulletSpec;
    public StackPool<Bullet> Bullets;

    public event Action OnRecall;
    [ContextMenu("Recall Pools")]
    public void Recall()
    {
        OnRecall?.Invoke();
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // End of Singleton protection
        DontDestroyOnLoad(gameObject);
        // Initialize all pools

        Bullets = InitPool(_bulletSpec);
        
    }

    private StackPool<T> InitPool<T>(PoolSpec<T> pool) where T : MonoBehaviour
    {
        return new StackPool<T>(pool.amount, pool.prefab, transform);
    }
    /// <summary>
    /// Sets active to false and releases the object if the object is in the world.
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <param name="targetPool"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool TryRelease<X>(StackPool<X> targetPool, X target) where X : MonoBehaviour
    {
        if (target.gameObject.activeSelf)
        {
            target.gameObject.SetActive(false);
            targetPool.ReleaseTo(target);
            return true;
        }
        return false;
    }
}

public class StackPool<T> where T : MonoBehaviour
{
    private Stack<T> _pool;
    private T _prefab;
    private int _outsideCount;
    private Transform _parent;
    public StackPool(int amount, T prefab, Transform parent)
    {
        _pool = new Stack<T>();
        for (int i = 0; i < amount; i++)
        {
            _pool.Push(GameObject.Instantiate(prefab, parent));

        }
        _prefab = prefab;
        _parent = parent;
    }
    public T GetFrom()
    {
        _outsideCount++;
        if (_pool.TryPop(out T result))
        {
            return result;
        }
        else
        {
            return GameObject.Instantiate(_prefab, _parent);
        }
    }
    public void ReleaseTo(T released)
    {
        _outsideCount--;
        _pool.Push(released);
    }
    public int GetOutsideCount()
    {
        return _outsideCount;
    }
}
