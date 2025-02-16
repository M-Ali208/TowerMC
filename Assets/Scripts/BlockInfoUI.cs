using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlockInfoUI : MonoBehaviour
{
    public GameObject infoPanel;  // UI Panel
    public TMP_Text blockNameText; // Blok Adı
    public Image blockImage; // Blok Görseli
    private bool isVisible = false;

    public GameObject _blockToPlace; // Önizlenen blok (private değişken)

    // Önizleme bloğu değiştirildiğinde çağrılacak event
    public void SetBlockToPlace(GameObject newBlock)
    {
        if (newBlock == _blockToPlace) return; // Aynı bloksa güncelleme yapma

        _blockToPlace = newBlock;
        UpdateBlockInfo();
    }

    void Start()
    {
        if (infoPanel == null || blockNameText == null || blockImage == null)
        {
            Debug.LogError("UI bileşenleri eksik! Inspector'dan atamalarını yapın.");
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
        }
        if (isVisible) UpdateBlockInfo();
    }

    private void UpdateBlockInfo()
    {
        if (_blockToPlace == null)
        {
            Debug.LogError("Hata: Önizleme bloğu atanmadı!");
            return;
        }

        blockNameText.text = _blockToPlace.name;
        Debug.Log("Blok adı güncellendi: " + _blockToPlace.name);

        SpriteRenderer spriteRenderer = _blockToPlace.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            blockImage.sprite = spriteRenderer.sprite;
            blockImage.rectTransform.rotation = Quaternion.Euler(0, 0, _blockToPlace.transform.rotation.eulerAngles.z);
            Debug.Log("Blok görseli güncellendi: " + spriteRenderer.sprite.name);
        }
        else
        {
            Debug.LogError("Hata: Önizleme bloğunda SpriteRenderer bileşeni yok!");
        }
    }
}
