using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Terrain;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

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

    public CollectibleSpawner collectibleSpawner;

    private void Start()
    {
        // for collectible testing
        collectibleSpawner.SpawnCollectible(CollectibleType.b_BlockCollectible, new Vector3(-1, -18, 0));
    }

    private void OnDestroy()
    {
        
    }
}
