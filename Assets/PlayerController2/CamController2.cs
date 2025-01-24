using UnityEngine;

public class CamController2 : MonoBehaviour
{
    [Range(0, 1)]
    public float smoothTime;
    private float orthoSize;
    public Transform playerTransform;
    public void FixedUpdate()
    {
        orthoSize = GetComponent<Camera>().orthographicSize;
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);
        pos.x = Mathf.Clamp(pos.x, -60 + orthoSize + 4, 60 - orthoSize - 4);
        pos.y = Mathf.Clamp(pos.y, -80 + orthoSize, 160 - orthoSize);
        GetComponent<Transform>().position = pos;
    }
}
