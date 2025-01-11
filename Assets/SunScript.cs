using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10000f;
    private bool rotationChange;
    private Rigidbody2D rb;
    
    void Update()
    {
       rotationChange = Input.GetKeyDown(KeyCode.R);
        rb = GetComponent<Rigidbody2D>();
        if (rotationChange)
        {
            rb.angularVelocity = 0;
            rb.angularVelocity = moveSpeed * Time.deltaTime ;
           
        }

    }
    void Start()
    {

      

    }

    
}
