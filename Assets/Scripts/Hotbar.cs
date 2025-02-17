using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public List<GameObject> slots; // UI elemanlar�, hotbar slotlar�
    private Inventory inventory;

    public int activeSlotIndex = 0; // Aktif slot indexi

    void Start()
    {
        inventory = GetComponent<Inventory>();
        UpdateHotbar(activeSlotIndex); // Ba�lang��ta hotbar'� ba�lat
    }

    void Update()
    {
        // Mouse scroll hareketi ile slot de�i�tirme
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            ChangeActiveSlot((int)Mathf.Sign(scroll)); // Scroll ile aktif slotu de�i�tir
        }
    }

    public void UpdateHotbar(int currentSlotIndex)
    {
        // Hotbar UI's�n�, envanterdeki se�ilen blo�u g�sterecek �ekilde g�ncelle
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.placeableBlocks.Count)
            {
                // �lgili slotta blo�un simgesini g�ster
                GameObject blockPrefab = inventory.placeableBlocks[i];
                if (blockPrefab != null)
                {
                    Sprite blockSprite = blockPrefab.GetComponent<SpriteRenderer>().sprite;
                    slots[i].GetComponent<Image>().sprite = blockSprite;
                }
            }
            else
            {
                // Bo� slot
                slots[i].GetComponent<Image>().sprite = null;
            }
        }

        // Se�ilen slotu vurgula
        HighlightSlot(currentSlotIndex);
    }

    private void HighlightSlot(int currentSlotIndex)
    {
        // T�m slotlar� �nce normal hale getir
        foreach (var slot in slots)
        {
            slot.GetComponent<Image>().color = Color.white; // Normal renk
        }

        // Se�ilen slotu vurgula
        if (currentSlotIndex >= 0 && currentSlotIndex < slots.Count)
        {
            slots[currentSlotIndex].GetComponent<Image>().color = Color.yellow; // Se�ili slotu vurgula
        }
    }

    private void ChangeActiveSlot(int direction)
    {
        // Yeni aktif slotu hesapla
        activeSlotIndex += direction;

        // Slot indexini s�n�rlarla k�s�tla
        if (activeSlotIndex < 0) activeSlotIndex = inventory.placeableBlocks.Count - 1;
        if (activeSlotIndex >= inventory.placeableBlocks.Count) activeSlotIndex = 0;

        // Hotbar'� g�ncelle
        UpdateHotbar(activeSlotIndex);

        // Aktif blo�u se�
        inventory.blockHand = inventory.placeableBlocks[activeSlotIndex];
        PlayerController.instance.blockToPlace = inventory.blockHand;
    }
}
