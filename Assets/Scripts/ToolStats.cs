using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolStats : MonoBehaviour
{
    public CurrentToolMaterial currentToolMaterial;
    public ToolType toolType;
    public ToolMaterialSpeed toolMaterialSpeed;

    public void OnHand()
    {
        
        PlayerController.instance.currentToolMaterialTier = currentToolMaterial;
        PlayerController.instance.currentToolType = toolType;
        PlayerController.instance.currentToolMaterialSpeed = toolMaterialSpeed;
    }
}
