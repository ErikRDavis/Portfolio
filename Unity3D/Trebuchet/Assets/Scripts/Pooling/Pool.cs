using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Pool Instance { get; private set; }

    [SerializeField]
    private List<PoolItem> poolItems;

    private Dictionary<string, Queue<GameObject>> pool;
    private Dictionary<string, GameObject> prefabs;

    private void Awake()
    {
        Instance = this;
        pool = new Dictionary<string, Queue<GameObject>>();
        prefabs = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        PoolItem item;
        for (int i = 0; i < poolItems.Count; i++)
        {
            item = poolItems[i];

            if (item.prefab.GetComponent<IPoolItem>() != null)
            {
                CreatePooledItem(item);
            }
            else
            {
                Debug.LogError($"Item \"{item.prefab.name}\" does not implement the IPoolItem interface");
            }
        }
    }

    private void CreatePooledItem(PoolItem item)
    {
        if (pool.ContainsKey(item.itemKey) == false)
        {
            pool.Add(item.itemKey, new Queue<GameObject>());
            prefabs.Add(item.itemKey, item.prefab);
        }

        for (int i = 0; i < item.count; i++)
        {
            GameObject itemObject = InstantiateItem(item.itemKey, i);
            

            pool[item.itemKey].Enqueue(itemObject);
        }
    }

    private GameObject InstantiateItem(string itemKey, int index)
    {
        GameObject newItem = Instantiate(Instance.prefabs[itemKey], transform);
        newItem.GetComponent<IPoolItem>().PoolKey = itemKey;
        newItem.name = $"{itemKey}: {index}";
        newItem.SetActive(false);

        return newItem;
    }

    public static T SpawnItem<T>(string itemKey)
    {
        if (Instance.pool.ContainsKey(itemKey))
        {
            if(!Instance.pool[itemKey].TryDequeue(out GameObject item))
            {
                item = Instance.InstantiateItem(itemKey, -1);
                Debug.LogWarning($"Creating extra \"{itemKey}\" pool item");
            }

            item.SetActive(true);
            item.transform.SetParent(null);

            return item.GetComponent<T>();
        }

        Debug.LogError($"No pool items exist for key \"{itemKey}\"");

        return default;
    }

    public static void DespawnItem(IPoolItem item)
    {
        item.GameObject.SetActive(false);
        item.GameObject.transform.SetParent(Instance.transform);

        Instance.pool[item.PoolKey].Enqueue(item.GameObject);
    }
}
