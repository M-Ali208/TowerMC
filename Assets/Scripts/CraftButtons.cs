using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftButtons : MonoBehaviour
{
    public void OnClick()
    {
        GameObject.FindGameObjectWithTag("CraftManager").GetComponent<CraftManager>().Craft(gameObject.name);
    }
}
