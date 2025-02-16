using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlockInfoUI : MonoBehaviour
{
    public GameObject infoPanel;  // UI Panel
    public TMP_Text blockNameText; // Blok Adı
    public Image blockImage; // Blok Görseli
    private bool isVisible = false;
    private bool isNameChanged = true;
    private bool isImageChanged = true;

    public GameObject _blockToPlace; // Önizlenen blok (private değişken)

    // Önizleme bloğu değiştirildiğinde çağrılacak event
    public void SetBlockToPlace(GameObject newBlock)
    {
        if (newBlock == null)
        {
            Debug.LogError("Hata: Yeni blok atanmadı!");
            return;
        }

        if (_blockToPlace != null && newBlock.name == _blockToPlace.name)
        {
            // Sadece isim değiştiyse, isNameChanged ve isImageChanged bayraklarını ayarla
            isNameChanged = true;
            isImageChanged = true;
        }
        else
        {
            // Blok tamamen değiştiyse, tüm bilgileri güncelle
            _blockToPlace = newBlock;
            isNameChanged = true;
            isImageChanged = true;
        }

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
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            isImageChanged = true;
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

        // Blok ismi güncelle
        if (isNameChanged || blockNameText.text != _blockToPlace.name)
        {
            blockNameText.text = _blockToPlace.name;
            Debug.Log("Blok adı güncellendi: " + _blockToPlace.name);
            isNameChanged = false;
        }

        // Blok görseli güncelle
        SpriteRenderer spriteRenderer = _blockToPlace.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (isImageChanged || blockImage.sprite != spriteRenderer.sprite)
            {
                blockImage.sprite = spriteRenderer.sprite;
                blockImage.rectTransform.rotation = Quaternion.Euler(0, 0, _blockToPlace.transform.rotation.eulerAngles.z);
                Debug.Log("Blok görseli güncellendi: " + spriteRenderer.sprite.name);
                isImageChanged = false;
            }
        }
    }

}
