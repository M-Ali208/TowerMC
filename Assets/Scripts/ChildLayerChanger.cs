using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLayerChanger : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private LayerMask targetLayerMask;

    private int previousChildCount = 0;
    private int targetLayer;

    void Start()
    {
        targetLayer = GetLayerFromMask(targetLayerMask);
        SetChildProperties();
        previousChildCount = parent.transform.childCount;
    }

    void Update()
    {
        // Eðer child sayýsý deðiþmiþse güncelle
        if (parent.transform.childCount != previousChildCount)
        {
            SetChildProperties();
            previousChildCount = parent.transform.childCount;
        }
    }

    private void SetChildProperties()
    {
        foreach (Transform child in parent.transform)
        {
            // Layer'ý güncelle
            if (child.gameObject.layer != targetLayer)
            {
                child.gameObject.layer = targetLayer;
            }

            // BoxCollider2D kontrol et ve isTrigger ayarla
            BoxCollider2D boxCollider = child.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                if (parent.name == "BackPlane" || parent.name == "FrontPlane")
                {
                    boxCollider.isTrigger = true;
                }
                else if (parent.name == "Main")
                {
                    boxCollider.isTrigger = false;
                }
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
