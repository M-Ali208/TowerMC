using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Inventor
{
    public string blockName;
    [Range(0, 64)] public int count;
    public bool isUseable = true;
}

[Serializable]
public class PlaceBlocks
{
    public GameObject blockGO;
}
public class Inventory : MonoBehaviour
{
    public List<Inventor> inventors;
    public List<GameObject> placeableBlocks;
    public GameObject blockHand;
    public GameObject toolHand;
   

    private void Update()
    {
        for (int i = 0; i < placeableBlocks.Count; i++)
        {
            if (inventors.Find(obj => obj.blockName == placeableBlocks[i].name) != null)
            {
                blockHand = placeableBlocks[i];
            }
        }

        // bunu kaldırın
        if (blockHand != null)
        {
            PlayerController.instance.blockToPlace = blockHand;
        }
        if (toolHand != null)
        {
            toolHand.GetComponent<ToolStats>().OnHand();
        }
        //
        foreach (var item in inventors)
        {
            if (item.count <= 0)
            {
                item.isUseable = true;
                item.blockName = " ";
            }
        }
    }

    public void AddItem(string item)
    {
        for (int i = 0; i < inventors.Capacity; i++)
        {
            if (inventors[i].blockName == item)
            {
                if (inventors[i].count < 64)
                {
                    inventors[i].count++;
                    return;
                }
            }
            if (inventors[i].isUseable)
            {
                inventors[i].blockName = item;
                inventors[i].count++;

                placeableBlocks.Add(Resources.Load<GameObject>($"Prefabs/Blocks/{inventors[i].blockName}"));

                inventors[i].isUseable = false;
                return;
            }
        }
    }

    public bool isHaveBlock()
    {
        var temp = false;
        foreach (var item in placeableBlocks)
        {
            if (blockHand.name == item.name && inventors.Find(obj => obj.blockName == item.name) != null)
            {
                temp = true;
                break;
            }
            else
            {
                temp = false;
            }
        }
        return temp;
    }
}
