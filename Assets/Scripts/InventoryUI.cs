using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    [SerializeField]private GameObject inventoryUI;
    public List<GameObject> slots;
    private Inventory inventory;
    private BlockSO BlockSO;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    [System.Obsolete]
    private void Update() {
        if (Input.GetKeyDown("e") && !inventoryUI.active)
        {
            inventoryUI.SetActive(true);
            UpdateInventory();
        }
        else if (Input.GetKeyDown("e") && inventoryUI.active)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Blocks/{inventory.inventors[i].blockName}");
            

            if (prefab != null)
            {
                BlockSO blockData = prefab.GetComponent<Blocks>().blockSO;
                prefab = blockData.BlockToPlace;
                slots[i].GetComponent<Image>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
