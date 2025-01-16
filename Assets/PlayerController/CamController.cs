using System.Collections;

using UnityEngine;

public class CamController : MonoBehaviour
{

    [Range(0, 1)]
    public float smoothTime;

    public Transform playerTransform;
    [SerializeField] float camOffsety;


    public void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y + camOffsety, smoothTime);
        GetComponent<Transform>().position = pos;

    }

}
