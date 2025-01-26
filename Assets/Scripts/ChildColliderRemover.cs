using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderRemover : MonoBehaviour
/*
{

    [SerializeField] private GameObject parent;

    void Start()
    {
        SetChildCollidersToTrigger();
    }

    void Update()
    {
        SetChildCollidersToTrigger();
    }

    private void SetChildCollidersToTrigger()
    {
        foreach (Transform child in parent.transform)
        {
            BoxCollider boxCollider = child.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.isTrigger = true;
            }
        }
    }
}
*/
{
    [SerializeField] private GameObject parent;
    [SerializeField] private LayerMask targetLayerMask;

    private int previousChildCount = 0;

    void Start()
    {
        DisableChildCollidersTrigger();
        previousChildCount = parent.transform.childCount;
        Debug.Log($"Initial child count: {previousChildCount}");
    }

    void Update()
    {
        int currentChildCount = parent.transform.childCount;
        // Check if the number of children has changed
        if (currentChildCount != previousChildCount)
        {
            Debug.Log("Child count changed, updating colliders.");
            DisableChildCollidersTrigger();
            previousChildCount = currentChildCount;
            Debug.Log($"Updated child count: {previousChildCount}");
        }
    }

    private void DisableChildCollidersTrigger()
    {
        foreach (Transform child in parent.transform)
        {
            if (((1 << child.gameObject.layer) & targetLayerMask) != 0)
            {
                Debug.Log($"Disabling trigger for child object: {child.gameObject.name}");
                Collider[] colliders = child.GetComponents<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.isTrigger = false;
                }
            }
        }
    }
}
