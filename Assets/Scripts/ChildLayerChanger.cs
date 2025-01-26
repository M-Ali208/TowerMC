using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLayerChanger : MonoBehaviour
/*
  {
    [SerializeField] private GameObject parent;
    [SerializeField] private LayerMask targetLayerMask;

    private int previousChildCount = 0;

    void Start()
    {
        SetChildLayers();
        previousChildCount = parent.transform.childCount;
    }

    void Update()
    {
        // Check if the number of children has changed
        if (parent.transform.childCount != previousChildCount)
        {
            SetChildLayers();
            previousChildCount = parent.transform.childCount;
        }
    }

    private void SetChildLayers()
    {
        int targetLayer = Mathf.RoundToInt(Mathf.Log(targetLayerMask.value, 2));
        foreach (Transform child in parent.transform)
        {
            child.gameObject.layer = targetLayer;
        }
    }
}
*/
{
    [SerializeField] private GameObject parent;
    [SerializeField] private LayerMask targetLayerMask;

    private int previousChildCount = 0;
    private int targetLayer;

    void Start()
    {
        targetLayer = GetLayerFromMask(targetLayerMask);
        SetChildLayers();
        previousChildCount = parent.transform.childCount;
    }

    void Update()
    {
        // Check if the number of children has changed
        if (parent.transform.childCount != previousChildCount)
        {
            SetChildLayers();
            previousChildCount = parent.transform.childCount;
        }
    }

    private void SetChildLayers()
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.layer != targetLayer)
            {
                child.gameObject.layer = targetLayer;
            }
        }
    }

    private int GetLayerFromMask(LayerMask mask)
    {
        int layer = 0;
        int maskValue = mask.value;
        while (maskValue > 1)
        {
            maskValue = maskValue >> 1;
            layer++;
        }
        return layer;
    }
}
