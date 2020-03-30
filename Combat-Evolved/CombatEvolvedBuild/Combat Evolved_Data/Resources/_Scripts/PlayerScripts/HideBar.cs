using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideBar : MonoBehaviour
{
    public GameObject bars;
    // 0 transparent, 1 opaque
    public float transparency = .6f;
    float transitionDuration = .25f;

    PlayerController pc;
    HashSet<GameObject> offendors;
    List<Image> images;
    List<SpriteRenderer> srs;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        offendors = new HashSet<GameObject>();
        images = new List<Image>(bars.GetComponentsInChildren<Image>());
        srs = new List<SpriteRenderer>(bars.GetComponentsInChildren<SpriteRenderer>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("Player") || collision.CompareTag("UIBars")) && collision.GetComponentInParent<PlayerController>() != pc)
        {
            print(collision.name + offendors.Count);
            if(offendors.Count == 0)
            {
                SetTransparency(transparency);
            }
            offendors.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("UIBars")) && collision.GetComponentInParent<PlayerController>() != pc)
        {
            offendors.Remove(collision.gameObject);
            if (offendors.Count == 0)
            {
                SetTransparency(1);
            }
        }
    }

    private void SetTransparency(float f)
    {
        StopAllCoroutines();
        StartCoroutine(SetTransparencyEnumerator(f));
    }

    private IEnumerator SetTransparencyEnumerator(float f)
    {
        // set to opposite transparency
        float startingTransparency = f != 1 ? 1 : transparency;
        float deltaTransparency = (f - startingTransparency) * Time.deltaTime / transitionDuration;
        SetTransparencyForComponents(startingTransparency);
        float time = 0;
        while(time < transitionDuration)
        {
            SetTransparencyForComponents(startingTransparency);
            startingTransparency += deltaTransparency;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // clip to end
        SetTransparencyForComponents(f);
    }

    private void SetTransparencyForComponents(float f)
    {
        foreach (Image image in images)
        {
            Color c = image.color;
            c.a = f;
            image.color = c;
        }
        foreach (SpriteRenderer sr in srs)
        {
            Color c = sr.color;
            c.a = f;
            sr.color = c;
        }
    }

    public void ResetBars()
    {
        StopAllCoroutines();
        SetTransparencyForComponents(1);
    }
}
