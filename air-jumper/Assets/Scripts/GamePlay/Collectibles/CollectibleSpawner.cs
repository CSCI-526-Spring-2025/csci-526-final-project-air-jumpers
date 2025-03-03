using System.Collections;
using System.Collections.Generic;
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

    public void SpawnCollectible(CollectibleType collectibleType, Vector3 pos)
    {
        CollectibleScriptable collectibleData = collectibleDatabase.GetCollectibleByType(collectibleType);
        if (collectibleData != null)
        {
            GameObject collectibleObject = Instantiate(collectibleData.prefab, pos, Quaternion.identity);
            Collectible collectible = collectibleObject.AddComponent<Collectible>();
            collectible.collectibleData = collectibleData;
        }
    }
}
