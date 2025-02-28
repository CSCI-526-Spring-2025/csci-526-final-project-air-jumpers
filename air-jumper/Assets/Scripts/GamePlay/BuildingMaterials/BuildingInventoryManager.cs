using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInventoryManager : MonoBehaviour
{
    // Singleton
    public static BuildingInventoryManager Instance { get; private set; }

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

    public event Action<BuildingMaterialScriptable, int> OnMaterialUpdated;

    private Dictionary<BuildingMaterialScriptable, int> materials = new Dictionary<BuildingMaterialScriptable, int>();
    
    private void Start()
    {
        OnMaterialUpdated += OnMaterialUpdatedListener;
    }

    private void OnDestroy()
    {
        OnMaterialUpdated -= OnMaterialUpdatedListener;
    }

    public void AddBuildingMaterial(BuildingMaterialScriptable material, int amount = 1)
    {
        if (materials.ContainsKey(material))
        {
            materials[material] += amount;
        }
        else
        {
            materials[material] = amount;
        }

        OnMaterialUpdated?.Invoke(material, materials[material]);
    }

    public bool TryUsingMaterial(BuildingMaterialScriptable material, int amount = 1)
    {
        if (materials.ContainsKey(material) && materials[material] >= amount)
        {
            materials[material] -= amount;
            OnMaterialUpdated?.Invoke(material, materials[material]);
            return true;
        }

        return false;
    }

    public Dictionary<BuildingMaterialScriptable, int> GetCurrentInventory()
    {
        return new Dictionary<BuildingMaterialScriptable, int>(materials);
    }

    private void OnMaterialUpdatedListener(BuildingMaterialScriptable material, int newCount)
    {
        Debug.Log($"Building material {material.materialName} updated with count of {newCount}");
    }
}
