using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour
{
    private SpriteRenderer sr;
    private float duration = 1f;
    [SerializeField] private GameObject End;
    [SerializeField] private GameObject Surface;
    [SerializeField] private GameObject Underground;
    [SerializeField] private GameObject Nether;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(fadeEffect());
            if (gameObject.CompareTag("End"))
            {
                End.SetActive(true);
                Surface.SetActive(false);
                Underground.SetActive(false);
                Nether.SetActive(false);
            }
            else if (gameObject.CompareTag("Surface"))
            {
                End.SetActive(false);
                Surface.SetActive(true);
                Underground.SetActive(false);
                Nether.SetActive(false);
            }
            else if (gameObject.CompareTag("Underground"))
            {
                End.SetActive(false);
                Surface.SetActive(false);
                Underground.SetActive(true);
                Nether.SetActive(false);
            }
            else if (gameObject.CompareTag("Nether"))
            {
                End.SetActive(false);
                Surface.SetActive(false);
                Underground.SetActive(false);
                Nether.SetActive(true);
            }
        }
    }
    private IEnumerator fadeEffect()
    {
        float time = 0.0f;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(1, 0, time / duration);
            Color newColor = sr.color;
            newColor.a = alpha;
            sr.color = newColor;
            time += Time.deltaTime;
            yield return null;
        } 
    }
}
