using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DefaultBlockData", menuName = "Block Data", order = 51)]
public class BlockSO : ScriptableObject
{
    public string BlockName;
    public float Hardness;
    public ToolMaterial MinHarvestToolTier;
    public ToolMaterialSpeed ToolBreakSpeed;
    public ToolType BestToolType;
    public bool IsTrigger;
    public bool IsPlaceable;
    public AudioClip BreakSound;
    public AudioClip PlaceSound;
    public AudioClip WalkingSound;
    //lootTable

}

public enum ToolType
    {
        Hand,
        Pickaxe,
        Shovel,
        Axe,
        Hoe
    }

    public enum ToolMaterial
    {
        Hand,
        Wood,
        Gold,
        Stone,
        Iron,
        Diamond,
        Netherite
    }
    public enum ToolMaterialSpeed
    {
        Hand,
        Wood,
        Stone,
        Iron,
        Diamond,
        Netherite,
        Gold
}
public static class ToolMaterialTiers
    {
        public static readonly Dictionary<ToolMaterial, int> MaterialTiers = new Dictionary<ToolMaterial, int>
        {
            { ToolMaterial.Hand, 1 },
            { ToolMaterial.Wood, 2 },
            { ToolMaterial.Gold, 2 },
            { ToolMaterial.Stone, 3 },
            { ToolMaterial.Iron, 4 },
            { ToolMaterial.Diamond, 5 },
            { ToolMaterial.Netherite, 5 }
        };
    }
public static class ToolMaterialBreakSpeed
{
    public static readonly Dictionary<ToolMaterialSpeed, int> BreakSpeed = new Dictionary<ToolMaterialSpeed, int>
    {
        { ToolMaterialSpeed.Hand, 1 },
        { ToolMaterialSpeed.Wood, 2 },
        { ToolMaterialSpeed.Stone, 4 },
        { ToolMaterialSpeed.Iron, 6 },
        { ToolMaterialSpeed.Diamond, 8 },
        { ToolMaterialSpeed.Netherite, 9 },
        { ToolMaterialSpeed.Gold, 12 }
    };
}

/*
public void ApplyToCollider(BoxCollider2D collider)
{
    if (collider != null)
    {
        collider.isTrigger = IsTrigger;
    }
}

public bool CanBeHarvestedWith(ToolMaterial toolMaterial)
{
    if (ToolMaterialTiers.MaterialTiers.TryGetValue(toolMaterial, out int toolTier) &&
        ToolMaterialTiers.MaterialTiers.TryGetValue(MinHarvestToolTier, out int minToolTier))
    {
        return toolTier >= minToolTier;
    }
    return false;
}
public void PlayBreakSound(AudioSource audioSource)
    {
        if (BreakSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(BreakSound);
        }
    }

    public void PlayPlaceSound(AudioSource audioSource)
    {
        if (PlaceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(PlaceSound);
        }
    }
*/