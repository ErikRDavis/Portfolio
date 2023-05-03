using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public string itemKey;
    public GameObject prefab;
    public int count;
}

public interface IPoolItem
{
    public string PoolKey { get; set; }
    public GameObject GameObject { get; }

    public void Despawn();
}
