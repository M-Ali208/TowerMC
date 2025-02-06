using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject inventoryPanel; // UI Panel'i
    private bool isOpen = false; // Açýk mý kapalý mý?
    public bool isGameRunning = true; // Oyun oynanýyor mu?

    void Update()
    {
        if (isGameRunning) return; // Eðer oyun oynanýyorsa envanter açýlmasýn

        if (Input.GetKeyDown(KeyCode.E)) // E tuþuna basýnca
        {
            isOpen = !isOpen; // Aç/Kapat
            inventoryPanel.SetActive(isOpen);
        }
    }
}
