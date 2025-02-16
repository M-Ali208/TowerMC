using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlockInfoUI : MonoBehaviour
{
    public GameObject infoPanel;  // UI Panel
    public TMP_Text blockNameText; // Blok Ad�
    public Image blockImage; // Blok G�rseli
    public GameObject blockToPlace; // �nizlenen blok
    private bool isVisible = false;

    void Start()
    {
        if (infoPanel == null || blockNameText == null || blockImage == null)
        {
            Debug.LogError("UI bile�enleri eksik! Inspector'dan atamalar�n� yap�n.");
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
            Debug.LogError("Hata: �nizleme blo�u atanmad�!");
            return;
        }

        blockNameText.text = blockToPlace.name;
        Debug.Log("Blok ad� g�ncellendi: " + blockToPlace.name);

        SpriteRenderer spriteRenderer = blockToPlace.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            blockImage.sprite = spriteRenderer.sprite;

            // UI'daki Image'in d�n�� a��s�n� g�ncelle
            blockImage.rectTransform.rotation = Quaternion.Euler(0, 0, blockToPlace.transform.rotation.eulerAngles.z);

            Debug.Log("Blok g�rseli g�ncellendi: " + spriteRenderer.sprite.name);
        }
        else
        {
            Debug.LogError("Hata: �nizleme blo�unda SpriteRenderer bile�eni yok!");
        }
    }

}

