using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/SkillCollectible")]
public class SkillCollectibleScriptable : CollectibleScriptable
{
    private void OnEnable()
    {
        category = CollectibleCategory.Skill;
    }

    public override void ApplyEffect(GameObject player)
    {
        switch (collectibleType)
        {
            case CollectibleType.s_PlatformCollectible:
                player.GetComponent<PlayerMovement>().AddPlatform(amount);
                break; 
            default:
                break;
        }
    }
}
