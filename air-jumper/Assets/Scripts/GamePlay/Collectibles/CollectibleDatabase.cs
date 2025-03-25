using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    b_BlockCollectible,
    b_GunCollectible,
    b_DashCollectible,
    s_PlatformCollectible,
}


[System.Serializable]
public class CollectibleEntry
{
    public CollectibleType collectibleType;
    public CollectibleScriptable collectibleData;
}


[CreateAssetMenu(fileName = "CollectibleDatabase", menuName = "Collectible Database")]
public class CollectibleDatabase : ScriptableObject
{
    public List<CollectibleEntry> collectibleList = new List<CollectibleEntry>();

    private Dictionary<CollectibleType, CollectibleScriptable> collectibleLibrary = new Dictionary<CollectibleType, CollectibleScriptable>();

    private void OnEnable()
    {
        foreach (CollectibleEntry entry in collectibleList)
        {
            if (!collectibleLibrary.ContainsKey(entry.collectibleType))
            {
                entry.collectibleData.collectibleType = entry.collectibleType;
                collectibleLibrary.Add(entry.collectibleType, entry.collectibleData);
            }
        }
    }

    public CollectibleScriptable GetCollectibleByType(CollectibleType type)
    {
        if (collectibleLibrary == null || collectibleLibrary.Count == 0)
        {
            return null;
        }

        if (collectibleLibrary.TryGetValue(type, out CollectibleScriptable scriptable))
        {
            return scriptable;
        }

        return null;
    }
}
