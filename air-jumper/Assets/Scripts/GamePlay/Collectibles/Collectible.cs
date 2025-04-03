using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleScriptable collectibleData;
    public int times = 0;

    private CollectibleScriptable activeCollectibleData;

    void OnEnable()
    {
        if (collectibleData != null)
        {
            SetCollectibleData(collectibleData);
        }
    }

    public void SetCollectibleData(CollectibleScriptable data)
    {
        activeCollectibleData = Instantiate(data);
        if (times > 0)
        {
            activeCollectibleData.amount = times;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyCollectibleEffect(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void ApplyCollectibleEffect(GameObject player)
    {
        activeCollectibleData.ApplyEffect(player);

        if (CollectibleSpawner.Instance != null)
        {
            CollectibleSpawner.Instance.CollectCollectible(activeCollectibleData.collectibleType, transform.position);
        }
    }
}
