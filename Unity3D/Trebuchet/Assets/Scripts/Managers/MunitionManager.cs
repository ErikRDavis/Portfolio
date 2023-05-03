using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunitionManager : MonoBehaviour
{
    public delegate void OnMunitionSpawned(Munition munition);
    public event OnMunitionSpawned onMunitionSpawned;
    public delegate void OnMunitionDespawned(Munition munition);
    public event OnMunitionDespawned onMunitionDespawned;

    public static MunitionManager Instance { get; private set; }

    [SerializeField]
    private float maxMunitionLifeTime = 300;
    [SerializeField]
    private float despawnCheckFrequency = 1;

    private Dictionary<int, Munition> trackedMunitions;
    private Dictionary<int, double> despawnTimestamps;
    private List<int> munitionsToDespawn;

    private double Timestamp => Time.realtimeSinceStartupAsDouble;

    private void Awake()
    {
        Instance = this;

        trackedMunitions = new Dictionary<int, Munition>();
        despawnTimestamps = new Dictionary<int, double>();
        munitionsToDespawn = new List<int>();
    }

    private void Start()
    {
        InvokeRepeating("CheckForDespawn", despawnCheckFrequency, despawnCheckFrequency);
    }

    private void CheckForDespawn()
    {
        foreach (KeyValuePair<int, double> timestamp in despawnTimestamps)
        {
            if (Timestamp - timestamp.Value > maxMunitionLifeTime)
            {
                munitionsToDespawn.Add(timestamp.Key);
            }
        }

        for (int i = 0; i < munitionsToDespawn.Count; i++)
        {
            DespawnMunition(munitionsToDespawn[i]);
        }

        munitionsToDespawn.Clear();
    }

    private void DespawnMunition(int munitionId)
    {
        if(trackedMunitions.ContainsKey(munitionId))
        {
            Munition munition = trackedMunitions[munitionId];

            Pool.DespawnItem(munition);
            trackedMunitions.Remove(munitionId);

            Instance.onMunitionDespawned?.Invoke(munition);
        }
        else
        {
            Debug.LogError($"Munition Manager - DespawnMunition() - No tracked munition with instance ID: {munitionId}");
        }

        despawnTimestamps.Remove(munitionId);
    }

    public static Munition GetMunition(string munitionKey)
    {
        Munition munition = Pool.SpawnItem<Munition>(munitionKey);

        Instance.TrackMunition(munition);

        Instance.onMunitionSpawned?.Invoke(munition);

        return munition;
    }

    private void TrackMunition(Munition munition)
    {
        AddToTracked(munition);
    }

    private void AddToTracked(Munition munition)
    {
        if(trackedMunitions.TryAdd(munition.GetInstanceID(), munition) == false)
        {
            Debug.LogError($"Munition Manager - Already tracking munition with instance ID: {munition.GetInstanceID()}");
        }
    }

    public static void DespawnAfterLifetime(Munition munition)
    {
        Instance.UpdateDespawnTimestamp(munition.GetInstanceID(), Instance.Timestamp);
    }

    private void UpdateDespawnTimestamp(int instanceID, double timestamp)
    {
        if (despawnTimestamps.TryAdd(instanceID, timestamp) == false)
        {
            despawnTimestamps[instanceID] = timestamp;
        }
    }

    public static void WaitThenDespawnMunition(float time, Munition munition)
    {
        int key = munition.GetInstanceID();
        double despawnTimestamp = Instance.Timestamp - Instance.maxMunitionLifeTime + time;

        Instance.UpdateDespawnTimestamp(key, despawnTimestamp);
    }

}
