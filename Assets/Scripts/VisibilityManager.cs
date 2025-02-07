using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisibilityManager : MonoBehaviour
{
    [SerializeField]private GameObject[] gameObjects;

    [SerializeField]private List<GameObject> graundChunk;
    [SerializeField]private List<GameObject> graundChunk1;
    [SerializeField]private List<GameObject> graundChunk2;

    [SerializeField]private List<GameObject> caveChunk;
    [SerializeField]private List<GameObject> caveChunk1;
    [SerializeField]private List<GameObject> caveChunk2;

    [SerializeField]private List<GameObject> netherChunk;
    [SerializeField]private List<GameObject> netherChunk1;
    [SerializeField]private List<GameObject> netherChunk2;
    [SerializeField]private GameObject player;
    public bool caveOpenBool = true;
    public bool netherOpenBool = true;
    public bool caveCloseBool = true;

    private void Awake() {
        gameObjects = FindObjectsOfType<GameObject>();
        player = GameObject.FindWithTag("Player");
        foreach (var item in gameObjects)
        {   

            if (item.tag != "Player" && item.tag != "Grid")
            {
                if (item.transform.position.y >= 30)
                {
                    if (item.transform.position.x >= -60 && item.transform.position.x < -20)
                    {
                        graundChunk.Add(item);
                    }
                    else if (item.transform.position.x >= -20 && item.transform.position.x < 20)
                    {
                        graundChunk1.Add(item);
                    }
                    else
                    {
                        graundChunk2.Add(item);
                    }
                }
                else if (item.transform.position.y >= -28 && item.transform.position.y < 30)
                {
                    if (item.transform.position.x >= -60 && item.transform.position.x < -20)
                    {
                        caveChunk.Add(item);
                    }
                    else if (item.transform.position.x >= -20 && item.transform.position.x < 20)
                    {
                        caveChunk1.Add(item);
                    }
                    else
                    {
                        caveChunk2.Add(item);
                    }
                }
                else if(item.transform.position.y <= -28)
                {
                    if (item.transform.position.x >= -60 && item.transform.position.x < -20)
                    {
                        netherChunk.Add(item);
                    }
                    else if (item.transform.position.x >= -20 && item.transform.position.x < 20)
                    {
                        netherChunk1.Add(item);
                    }
                    else
                    {
                        netherChunk2.Add(item);
                    }
                }
    
                /*if (Mathf.Abs(item.transform.position.y - 40) >= player.transform.position.y)
                {
                   // item.SetActive(false);
                }*/
            }
        }


    }

    private void Start() {
        foreach (var item in caveChunk)
        {
           item.SetActive(false);
        }
        foreach (var item in caveChunk1)
        {
           item.SetActive(false);
        }

        foreach (var item in caveChunk2)
        {
           item.SetActive(false);
        }

        foreach (var item in netherChunk)
        {
            item.SetActive(false);
        }
        foreach (var item in netherChunk1)
        {
            item.SetActive(false);
        }
        foreach (var item in netherChunk2)
        {
            item.SetActive(false);
        }
    }
    
    private void Update() {
        if (player.transform.position.x >= -60 && player.transform.position.x < -20)
        {
            foreach (var item in graundChunk)
            {
                item.SetActive(false);
            }
            if (caveOpenBool)
            {
                foreach (var item in caveChunk)
                {
                    item.SetActive(false);
                }
            }
            else if (netherOpenBool)
            {
                foreach (var item in netherChunk)
                {
                    item.SetActive(false);
                }
            }
        }
        else if (player.transform.position.x >= -20 && player.transform.position.x < 20)
        {
            foreach (var item in graundChunk1)
            {
                item.SetActive(false);
            }
            if (caveOpenBool)
            {
                foreach (var item in caveChunk1)
                {
                    item.SetActive(false);
                }
            }
            else if (netherOpenBool)
            {
                foreach (var item in netherChunk1)
                {
                    item.SetActive(false);
                }
            }
        }
        else
        {
            foreach (var item in graundChunk2)
            {
                item.SetActive(false);
            }
            if (caveOpenBool)
            {
                foreach (var item in caveChunk2)
                {
                    item.SetActive(false);
                }
            }
            else if (netherOpenBool)
            {
                foreach (var item in netherChunk2)
                {
                    item.SetActive(false);
                }
            }
        }

        if (player.transform.position.y <= 35 && caveOpenBool)
        {
            caveOpenBool = false;
            foreach (var item in caveChunk)
            {
                item.SetActive(true);
            }
        }
        else if (player.transform.position.y <= -10 && netherOpenBool && player.transform.position.y > -20)
        {
            netherOpenBool = false;
            foreach (var item in netherChunk)
            {
                item.SetActive(true);
            }
        }
        else if (player.transform.position.y <= -33 && caveCloseBool)
        {
            caveCloseBool = false;
            foreach (var item in caveChunk)
            {  
                if (item != null)
                {
                    item.SetActive(false);
                }
            }
        }
    }


}
