using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    [SerializeField]private GameObject[] gameObjects;
    [SerializeField]private GameObject player;

    private void Awake() {
        gameObjects = FindObjectsOfType<GameObject>();
        player = GameObject.FindWithTag("Player");
        foreach (var item in gameObjects)
        {
            if (Mathf.Abs(item.transform.position.y - 40) >= player.transform.position.y)
            {
                item.SetActive(false);
            }
        }
    }


}
