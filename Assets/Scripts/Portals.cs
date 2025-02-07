using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour
{
    [SerializeField]private Transform portalTransform;

    private void OnTriggerEnter2D(Collider2D other) {  
        other.transform.position = portalTransform.position;
    }

}
