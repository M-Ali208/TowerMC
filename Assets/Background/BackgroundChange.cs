using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour

{
    private float duration = 4f;
    [SerializeField] private GameObject End;
    [SerializeField] private GameObject Surface;
    [SerializeField] private GameObject Underground;
    [SerializeField] private GameObject Nether;
    [SerializeField] private List<string> targetTags; // Fade effect uygulanacak tag'ler

    List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    List<SpriteRenderer> currentActiveRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        // sr bileþenini kaldýrdýk çünkü artýk fade effecti belirli tag'lere sahip objelere uygulayacaðýz
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HandleFade());
        }
    }

    private IEnumerator HandleFade()
    {
        // Mevcut aktif objelerin SpriteRenderer bileþenlerini al
        currentActiveRenderers.Clear();
        if (End.activeSelf) AddChildSpriteRenderers(End.transform, currentActiveRenderers);
        if (Surface.activeSelf) AddChildSpriteRenderers(Surface.transform, currentActiveRenderers);
        if (Underground.activeSelf) AddChildSpriteRenderers(Underground.transform, currentActiveRenderers);
        if (Nether.activeSelf) AddChildSpriteRenderers(Nether.transform, currentActiveRenderers);

        // Yeni aktif olacak objelerin SpriteRenderer bileþenlerini al
        spriteRenderers.Clear();
        foreach (string tag in targetTags)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in taggedObjects)
            {
                AddChildSpriteRenderers(obj.transform, spriteRenderers);
            }
        }

        // FadeOut ve FadeIn coroutine'lerini ayný anda baþlat
        StartCoroutine(FadeOut());
        StartCoroutine(FadeIn());

        // Yeni aktif objeleri ayarla
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

        yield return null;
    }

    private IEnumerator FadeIn()
    {
        float time = 0.0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(0, 1, time / duration);
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                Color newColor = sr.color;
                newColor.a = alpha;
                sr.color = newColor;
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float time = 0.0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(1, 0, time / duration);
            foreach (SpriteRenderer sr in currentActiveRenderers)
            {
                Color newColor = sr.color;
                newColor.a = alpha;
                sr.color = newColor;
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void AddChildSpriteRenderers(Transform parent, List<SpriteRenderer> spriteRenderers)
    {
        foreach (Transform child in parent)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spriteRenderers.Add(sr);
            }
            // Recursive call to add sprite renderers of all children
            AddChildSpriteRenderers(child, spriteRenderers);
        }
    }
}