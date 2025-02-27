using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BuildingMaterial")]
public class BuildingMaterialScriptable : ScriptableObject
{
    public string materialName;
    public GameObject prefab;

    public Vector2 snapSize = Vector2.one;
}
