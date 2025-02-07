using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject inventoryPanel; // UI Panel'i
    private bool isOpen = false; // A��k m� kapal� m�?
    public bool isGameRunning = true; // Oyun oynan�yor mu?

    void Update()
    {
        if (isGameRunning) return; // E�er oyun oynan�yorsa envanter a��lmas�n

        if (Input.GetKeyDown(KeyCode.E)) // E tu�una bas�nca
        {
            isOpen = !isOpen; // A�/Kapat
            inventoryPanel.SetActive(isOpen);
        }
    }
}
