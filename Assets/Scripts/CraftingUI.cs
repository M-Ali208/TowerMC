using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject CraftPanel; 
    public List<Button> craftButtons;
    private CraftManager craftManager;
    private BlockSO BlockSO;
    private void Start() {
       craftManager = gameObject.GetComponent<CraftManager>();

        for (int i = 0; i < craftButtons.Count; i++)
        {
            craftButtons[i].name = craftManager.craftLists[i].itemName;
            
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Blocks/{craftButtons[i].name}");
            //blockso ile iliþkilendirlecek yoksa build alamýyok
            if (prefab != null)
            {
                BlockSO blockData = prefab.GetComponent<Blocks>().blockSO;
                prefab = blockData.BlockToPlace;
                craftButtons[i].GetComponent<Image>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
            
            
        }
       
    }

    [Obsolete]
    private void Update() {
        LoadCraftables();
        if (Input.GetKeyDown("c"))
        {
            if (CraftPanel.active != false)
            {
                CraftPanel.SetActive(false);
            }
            else
            {
                CraftPanel.SetActive(true);
            }

        }
    }

    public void LoadCraftables()
    {
        craftManager.Craftables();
        foreach (var item in craftManager.craftLists)
        {
            if (item.isCraftable)
            {
                foreach (var item2 in craftButtons)
                {
                    if (item2.name == item.itemName)
                    {
                        item2.GetComponent<Button>().interactable = true;
                        break;
                    }
                }
            }
            if (!item.isCraftable)
            {
                foreach (var item2 in craftButtons)
                {
                    if (item2.name == item.itemName)
                    {
                        item2.GetComponent<Button>().interactable = false;
                        break;
                    }
                }
            }
        }
    }

}
