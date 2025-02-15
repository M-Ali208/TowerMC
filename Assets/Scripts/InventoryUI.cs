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
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Blocks/{inventory.inventors[i].blockName}.prefab");
            if (prefab != null)
            {
                slots[i].GetComponent<Image>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
