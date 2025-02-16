using UnityEngine;
using TMPro;

public class DebugCoordinates : MonoBehaviour
{
    public PlayerController playerController;
    public TMP_Text coordinatesText;
    private bool isVisible = false;

    void Start()
    {
        coordinatesText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            isVisible = !isVisible;
            coordinatesText.gameObject.SetActive(isVisible);
        }

        if (isVisible && playerController != null)
        {
            Vector3 playerPos = playerController.transform.position;
            Vector3 roundedMousePos = playerController.GetRoundedPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            float blockRotation = playerController.blockRotationAngle;

            coordinatesText.text = $"Player Pos: {playerPos}\n" +
                                   $"Mouse Rounded Pos: {roundedMousePos}\n" +
                                   $"Block Rotation: {blockRotation}°";
        }
    }
}
