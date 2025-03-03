using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/SkillCollectible")]
public class SkillCollectibleScriptable : CollectibleScriptable
{
    private void OnEnable()
    {
        type = CollectibleCategory.Skill;
    }

    public override void ApplyEffect(GameObject player)
    {

    }
}
