using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventor
{
    public string blockName;
    public int count;
    public bool isUseable = true;
}


public class Inventory : MonoBehaviour
{
    public Rigidbody2D rb;
    public List<Inventor> inventors;
    public GameObject blockHand;
    public GameObject toolHand;

    public void AddItem(string item)
    {
        for (int i = 0; i < inventors.Capacity; i++)
        {
            if (inventors[i].blockName == item)
            {
                if (inventors[i].count < 64)
                {
                    inventors[i].count++;
                    return;
                }
            }
            if (inventors[i].isUseable)
            {
                inventors[i].blockName = item;
                inventors[i].count++;
                inventors[i].isUseable = false;
                return;
            }
        }
    }

}
