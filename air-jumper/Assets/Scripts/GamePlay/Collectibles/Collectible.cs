using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleScriptable collectibleData;

    // Update is called once per frame
    void Update()
    {
        
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
        collectibleData.ApplyEffect(player);
    }
}
