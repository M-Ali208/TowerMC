using UnityEngine;
using UnityEngine.UIElements;
public class Parallax : MonoBehaviour
{

    private float lengthx, lengthy, startPosx, startPosy;
    public GameObject cam;
    public float parallaxEffect ;

    void Start()
    {
        startPosx = transform.position.x;
        lengthx  = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosy = transform.position.y;
        lengthy = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void FixedUpdate()
    {
        float tempx = (cam.transform.position.x * (1 - parallaxEffect));
        float distx = (cam.transform.position.x * parallaxEffect);
        float tempy = (cam.transform.position.y * (1 - parallaxEffect));
        float disty = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(startPosx + distx, transform.position.y, transform.position.z);
        transform.position = new Vector3(startPosy + disty, transform.position.x, transform.position.z);

        if (tempx > startPosx + lengthx) startPosx += lengthx;
        else if (tempx < startPosx - lengthx) startPosx -= lengthx;

        if (tempy > startPosy + lengthy) startPosy += lengthy;
        else if (tempy < startPosy - lengthy) startPosy -= lengthy;
    }

}