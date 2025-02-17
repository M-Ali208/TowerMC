using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public List<GameObject> slots; // UI elemanlarý, hotbar slotlarý
    private Inventory inventory;

    public int activeSlotIndex = 0; // Aktif slot indexi

    void Start()
    {
        inventory = GetComponent<Inventory>();
        UpdateHotbar(activeSlotIndex); // Baþlangýçta hotbar'ý baþlat
    }

    void Update()
    {
        // Mouse scroll hareketi ile slot deðiþtirme
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            ChangeActiveSlot((int)Mathf.Sign(scroll)); // Scroll ile aktif slotu deðiþtir
        }
    }

    public void UpdateHotbar(int currentSlotIndex)
    {
        // Hotbar UI'sýný, envanterdeki seçilen bloðu gösterecek þekilde güncelle
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.placeableBlocks.Count)
            {
                // Ýlgili slotta bloðun simgesini göster
                GameObject blockPrefab = inventory.placeableBlocks[i];
                if (blockPrefab != null)
                {
                    Sprite blockSprite = blockPrefab.GetComponent<SpriteRenderer>().sprite;
                    slots[i].GetComponent<Image>().sprite = blockSprite;
                }
            }
            else
            {
                // Boþ slot
                slots[i].GetComponent<Image>().sprite = null;
            }
        }

        // Seçilen slotu vurgula
        HighlightSlot(currentSlotIndex);
    }

    private void HighlightSlot(int currentSlotIndex)
    {
        // Tüm slotlarý önce normal hale getir
        foreach (var slot in slots)
        {
            slot.GetComponent<Image>().color = Color.white; // Normal renk
        }

        // Seçilen slotu vurgula
        if (currentSlotIndex >= 0 && currentSlotIndex < slots.Count)
        {
            slots[currentSlotIndex].GetComponent<Image>().color = Color.yellow; // Seçili slotu vurgula
        }
    }

    private void ChangeActiveSlot(int direction)
    {
        // Yeni aktif slotu hesapla
        activeSlotIndex += direction;

        // Slot indexini sýnýrlarla kýsýtla
        if (activeSlotIndex < 0) activeSlotIndex = inventory.placeableBlocks.Count - 1;
        if (activeSlotIndex >= inventory.placeableBlocks.Count) activeSlotIndex = 0;

        // Hotbar'ý güncelle
        UpdateHotbar(activeSlotIndex);

        // Aktif bloðu seç
        inventory.blockHand = inventory.placeableBlocks[activeSlotIndex];
        PlayerController.instance.blockToPlace = inventory.blockHand;
    }
}
