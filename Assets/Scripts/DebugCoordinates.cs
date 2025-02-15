using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class DebugCoordinates : MonoBehaviour
{
    public PlayerController pc;
    public TMP_Text coordinatesText;
    private bool isVisible = false;
    private Vector2 gridOffset = new Vector2(0.5f, 0.5f);
    private float gridSize = 1.0f;


    void Start()
    {
        coordinatesText.gameObject.SetActive(false);
    }
    private Vector3 mausePos(Vector3 position)

    {
        position.x = Mathf.Round((position.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x;
        position.y = Mathf.Round((position.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y;
        return position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            isVisible = !isVisible;
            coordinatesText.gameObject.SetActive(isVisible);
        }

        if (isVisible)
        {
            Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

    }
}

