using UnityEngine;
using UnityEngine.UI;

public class PlaneSwitcher : MonoBehaviour
{
    public Image planeImage; 
    public Sprite mainSprite;
    public Sprite backplaneSprite;
    public Sprite frontplaneSprite;

    private int currentIndex = 0; 
    private Sprite[] planeOrder;  

    void Start()
    {
        planeOrder = new Sprite[] { backplaneSprite, mainSprite, frontplaneSprite };
        planeImage.sprite = planeOrder[currentIndex]; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            SwitchPlane();
        }
    }

    void SwitchPlane()
    {
        currentIndex = (currentIndex + 1) % planeOrder.Length; 
        planeImage.sprite = planeOrder[currentIndex]; 
    }
}
