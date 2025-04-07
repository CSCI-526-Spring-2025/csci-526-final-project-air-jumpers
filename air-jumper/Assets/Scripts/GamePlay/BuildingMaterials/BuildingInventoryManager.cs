using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInventoryManager : MonoBehaviour
{
    // Singleton
    public static BuildingInventoryManager Instance { get; private set; }

    private List<GameObject> placedPlatform = new List<GameObject>();

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
    public event Action<BuildingMaterialScriptable, int> OnMaterialCleared;

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
            // Decrease building platform count
            if (SendToGoogle.Instance != null)
            {
                SendToGoogle.Instance?.incrementBuildingPlatformCount();
            }

            // Decrease the material count
            materials[material] -= amount;
            OnMaterialUpdated?.Invoke(material, materials[material]);
            return true;
        }

        return false;
    }

    private void ClearMaterial(BuildingMaterialScriptable material)
    {
        OnMaterialCleared?.Invoke(material, 0);
    }

    public Dictionary<BuildingMaterialScriptable, int> GetCurrentInventory()
    {
        return new Dictionary<BuildingMaterialScriptable, int>(materials);
    }

    private void OnMaterialUpdatedListener(BuildingMaterialScriptable material, int newCount)
    {
        Debug.Log($"Building material {material.materialName} updated with count of {newCount}");
    }

    public void PlacePlatform(GameObject platform)
    {
        placedPlatform.Add(platform);
    }

    // Returns the list of placed platforms
    public List<GameObject> getPlacedPlatforms()
    {
        return placedPlatform;
    }

    // Returns the number of placed platforms
    public int getPlacedPlatformsCount()
    {
        return placedPlatform.Count;
    }


    public void Clear()
    {
        foreach (var material in materials)
        {
            ClearMaterial(material.Key);
        }
        materials.Clear();

        foreach (GameObject platform in placedPlatform)
        {
            try
            {
                Destroy(platform);
            }
            catch 
            { 
                //
            }
        }

        placedPlatform.Clear();
    }
}
