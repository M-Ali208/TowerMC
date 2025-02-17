using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RequireItems
{
    public string itemName;
    public int count;
}


[Serializable]
public class CraftList
{
    public string itemName;
    public int outPutCount;
    public List<RequireItems> requireItems;
    public bool isCraftable;
}

public class CraftManager : MonoBehaviour
{
    private Inventory playerInventory;
    public List<CraftList> craftLists;

    private void Start() {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void Craftables()
    {
        for (int x = 0; x < craftLists.Count; x++)
        { 
            bool[] tempBool = new bool[craftLists[x].requireItems.Count];
            for (int i = 0; i < playerInventory.inventors.Count; i++)
            {
               for (int y = 0; y < craftLists[x].requireItems.Count; y++)
               {
                    if (playerInventory.inventors[i].blockName == craftLists[x].requireItems[y].itemName && playerInventory.inventors[i].count >= craftLists[x].requireItems[y].count)
                    {
                        tempBool[y] = true;
                    }
               }
                if (Array.TrueForAll(tempBool, deger => deger))
                {
                    craftLists[x].isCraftable = true;
                }
                else
                {
                    craftLists[x].isCraftable = false;
                }
               
            }
        }
    }

    public void Craft(string blockName)
    {
        foreach (var item in craftLists)
        {
            if (item.itemName == blockName && item.isCraftable)
            {
                foreach (var item2 in item.requireItems)
                {
                    foreach (var item3 in playerInventory.inventors)
                    {
                        if (item2.itemName == item3.blockName)
                        {
                            item3.count -= item2.count;
                            goto ContinueRequireItems;
                        }
                        else if (item3 == playerInventory.inventors[63])
                        {
                            goto ContinuecraftList;
                        }
                    }
                    ContinueRequireItems:
                    ;
                    
                }
                for (int i = 0; i < item.outPutCount; i++)
                {
                    playerInventory.AddItem(item.itemName);
                }
                

                Craftables();
            }
            ContinuecraftList:
            ;
        }
    }
}
