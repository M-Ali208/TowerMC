using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;

    [SerializeField] private List<GameObject> graundChunk = new List<GameObject>();
    [SerializeField] private List<GameObject> graundChunk1 = new List<GameObject>();
    [SerializeField] private List<GameObject> graundChunk2 = new List<GameObject>();

    [SerializeField] private List<GameObject> caveChunk = new List<GameObject>();
    [SerializeField] private List<GameObject> caveChunk1 = new List<GameObject>();
    [SerializeField] private List<GameObject> caveChunk2 = new List<GameObject>();

    [SerializeField] private List<GameObject> netherChunk = new List<GameObject>();
    [SerializeField] private List<GameObject> netherChunk1 = new List<GameObject>();
    [SerializeField] private List<GameObject> netherChunk2 = new List<GameObject>();

    [SerializeField] private GameObject player;
    private bool caveShouldBeOpen;
    private bool netherShouldBeOpen;
    private float playerX, playerY;

    // Bu eşik değerler örnek; ihtiyaca göre ayarlayın:
    // Ground için x konumları:
    //   - Sol bölge: x < -20  
    //   - Orta bölge: -20 <= x < 10  
    //   - Sağ bölge: x >= 10  
    //
    // Cave için y koşulu: oyuncu y <= 35 ve y > -33 ise mağara görünür  
    // Nether için y koşulu: oyuncu y <= -10 ve y > -20 ise nether görünür

    private void Awake()
    {
        // Tüm sahne objelerini alıp uygun listelere ekleyelim.
        gameObjects = FindObjectsOfType<GameObject>();
        player = GameObject.FindWithTag("Player");
        
        foreach (var item in gameObjects)
        {
            // Oyuncu, Grid veya Kamera etiketli objeleri atla.
            if (item.CompareTag("Player") || item.CompareTag("Grid") || item.CompareTag("MainCamera") || item.CompareTag("Surface") || item.CompareTag("End") || item.CompareTag("Underground") || item.CompareTag("Nether"))
                continue;

            float y = item.transform.position.y;
            float x = item.transform.position.x;

            // Konum bilgisine göre kategorilendirme:
            if (y >= 30)
            {
                // Ground nesneleri
                if (x >= -60 && x < -20)
                    graundChunk.Add(item);
                else if (x >= -20 && x < 20)
                    graundChunk1.Add(item);
                else
                    graundChunk2.Add(item);
            }
            else if (y >= -28 && y < 30)
            {
                // Cave nesneleri
                if (x >= -60 && x < -20)
                    caveChunk.Add(item);
                else if (x >= -20 && x < 20)
                    caveChunk1.Add(item);
                else
                    caveChunk2.Add(item);
            }
            else // y <= -28
            {
                // Nether nesneleri
                if (x >= -60 && x < -20)
                    netherChunk.Add(item);
                else if (x >= -20 && x < 20)
                    netherChunk1.Add(item);
                else
                    netherChunk2.Add(item);
            }
        }
    }

    private void Start()
    {
        SetActiveForList(graundChunk1, true);
        // Başlangıçta cave ve nether bölümlerinin tüm parçalarını kapatalım.
        SetActiveForList(caveChunk, false);
        SetActiveForList(caveChunk1, false);
        SetActiveForList(caveChunk2, false);

        SetActiveForList(netherChunk, false);
        SetActiveForList(netherChunk1, false);
        SetActiveForList(netherChunk2, false);



        
        
    }

    private void LateUpdate()
    {
        
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        // --- Ground (zemin) parçalarının kontrolü ---
        if (playerX < -10)
        {
            SetActiveForList(graundChunk, true);
            SetActiveForList(graundChunk2, false);
        }
        else if (playerX < 10) // -20 <= x < 10
        {

            SetActiveForList(graundChunk, false);
            SetActiveForList(graundChunk2, false);
        }
        else // playerX >= 10
        {
            SetActiveForList(graundChunk2, true);
            SetActiveForList(graundChunk, false);
        }

        // --- Cave (mağara) parçalarının kontrolü ---
        // Örneğin, oyuncu 35'ten aşağı ve -33'ten yukarı ise mağara görünür.
        caveShouldBeOpen = (playerY <= 35 && playerY > -33);
        if (caveShouldBeOpen)
        {
            SetActiveForList(caveChunk1, true);
            if (playerX < -10)
            {
                SetActiveForList(caveChunk, true);
                SetActiveForList(caveChunk2, false);
            }
            else if (playerX < 10)
            {
                SetActiveForList(caveChunk, false);
                SetActiveForList(caveChunk2, false);
            }
            else
            {
                SetActiveForList(caveChunk2, true);
                SetActiveForList(caveChunk, false);
            }
        }
        else
        {
            // Mağara görünüm kapalıysa tüm cave parçalarını kapat.
            SetActiveForList(caveChunk, false);
            SetActiveForList(caveChunk1, false);
            SetActiveForList(caveChunk2, false);
        }

        // --- Nether parçalarının kontrolü ---
        // Örneğin, oyuncu -10 ile -20 arasında ise nether görünür.
        netherShouldBeOpen = (playerY < -20);
        if (netherShouldBeOpen)
        {   
            SetActiveForList(netherChunk1, true);
            if (playerX < -10)
            {
                SetActiveForList(netherChunk, true);
                SetActiveForList(netherChunk2, false);
            }
            else if (playerX < 10)
            {
                SetActiveForList(netherChunk, false);
                SetActiveForList(netherChunk2, false);
            }
            else
            {
                SetActiveForList(netherChunk2, true);
                SetActiveForList(netherChunk, false);
            }
        }
        else
        {
            // Nether görünüm kapalıysa tüm nether parçalarını kapat.
            SetActiveForList(netherChunk, false);
            SetActiveForList(netherChunk1, false);
            SetActiveForList(netherChunk2, false);
        }
    }

    /// <summary>
    /// Verilen listedeki tüm objelerin aktiflik durumunu ayarlar.
    /// </summary>
    /// <param name="list">İşlem yapılacak GameObject listesi</param>
    /// <param name="active">Aktif yapılacaksa true, kapatılacaksa false</param>
    private void SetActiveForList(List<GameObject> list, bool active)
    {
        foreach (var item in list)
        {
            if (item != null)
                item.SetActive(active);
        }
    }
}
