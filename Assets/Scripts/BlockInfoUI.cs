using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlockInfoUI : MonoBehaviour
{
    public GameObject infoPanel;  // UI Panel
    public TMP_Text blockNameText; // Blok Adý
    public Image blockImage; // Blok Görseli
    public GameObject blockToPlace; // Önizlenen blok
    private bool isVisible = false;

    void Start()
    {
        if (infoPanel == null || blockNameText == null || blockImage == null)
        {
            Debug.LogError("UI bileþenleri eksik! Inspector'dan atamalarýný yapýn.");
            return;
        }
        infoPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            isVisible = !isVisible;
            infoPanel.SetActive(isVisible);
            if (isVisible) UpdateBlockInfo();
        }
    }

    private void UpdateBlockInfo()
    {
        if (blockToPlace == null)
        {
            Debug.LogError("Hata: Önizleme bloðu atanmadý!");
            return;
        }

        blockNameText.text = blockToPlace.name;
        Debug.Log("Blok adý güncellendi: " + blockToPlace.name);

        SpriteRenderer spriteRenderer = blockToPlace.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            blockImage.sprite = spriteRenderer.sprite;

            // UI'daki Image'in dönüþ açýsýný güncelle
            blockImage.rectTransform.rotation = Quaternion.Euler(0, 0, blockToPlace.transform.rotation.eulerAngles.z);

            Debug.Log("Blok görseli güncellendi: " + spriteRenderer.sprite.name);
        }
        else
        {
            Debug.LogError("Hata: Önizleme bloðunda SpriteRenderer bileþeni yok!");
        }
    }

}

