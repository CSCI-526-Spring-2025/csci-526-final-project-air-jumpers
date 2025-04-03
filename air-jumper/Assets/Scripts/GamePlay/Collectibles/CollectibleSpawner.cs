using System;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public static CollectibleSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CollectibleDatabase collectibleDatabase;

    public event Action<CollectibleType, Vector3> OnCollectibleSpawned;

    public event Action<CollectibleType, Vector3> OnCollectibleCollected;

    public void SpawnCollectible(CollectibleType collectibleType, Vector3 pos, int num = 0)
    {
        CollectibleScriptable collectibleData = collectibleDatabase.GetCollectibleByType(collectibleType);
        if (collectibleData != null)
        {
            GameObject collectibleObject = Instantiate(collectibleData.prefab, pos, Quaternion.identity);

            BuildingInventoryManager.Instance.PlacePlatform(collectibleObject);

            Collectible collectible = collectibleObject.AddComponent<Collectible>();
            if (num > 0)
            {
                collectible.times = num;
            }

            collectible.SetCollectibleData(collectibleData);

            OnCollectibleSpawned?.Invoke(collectibleType, pos);
        }
    }

    public void CollectCollectible(CollectibleType collectibleType, Vector3 pos)
    {
        OnCollectibleCollected?.Invoke(collectibleType, pos);
    }
}
