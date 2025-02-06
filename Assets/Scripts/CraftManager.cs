using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RequireItems
{
    public string itemName;
    public int count;
    public bool isAvailable;
}

[Serializable]
public class CraftList
{
    public string itemName;
    public List<RequireItems> requireItems;
    public bool isCraftable;
}

public class CraftManager : MonoBehaviour
{
    private Inventory playerInventory;
    [SerializeField]private List<CraftList> craftLists;

    private void Start() {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void Update() {
        Craftables();
    }

    public void Craftables()
    {
        

        for (int x = 0; x < craftLists.Capacity; x++)
        { 
            for (int i = 0; i < playerInventory.inventors.Capacity; i++)
            {
               
                foreach (var item in craftLists[x].requireItems)
                {
                    if (playerInventory.inventors[i].blockName == item.itemName)
                    {
                        item.isAvailable = true;
                    }
                }

                /*if (playerInventory.inventors[i].blockName == craftLists[x].requireItems[y].itemName && playerInventory.inventors[i].count >= craftLists[x].requireItems[y].count)
                {
                    craftLists[x].isCraftable = true;
                }*/
               
            }
        }
    }
}
