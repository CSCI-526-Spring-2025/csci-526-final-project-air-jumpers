using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleCategory
{
    Skill,
    BuildingMaterial
}

public abstract class CollectibleScriptable : ScriptableObject
{
    public CollectibleCategory category;
    public GameObject prefab;
    public int amount = 1;

    public CollectibleType collectibleType;

    public abstract void ApplyEffect(GameObject player);
}
