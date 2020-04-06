using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextZoomIn : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> chars;
    public GameObject evolvedText;
    public float updateSpeedSecs;
    public ParticleSystem textHitParticle;
    private Vector3 particleOffset;
    private Vector3 evolvedTextFinalPos;

    void Start()
    {
        particleOffset = new Vector3(0f, -1f, 0f);
        evolvedTextFinalPos = evolvedText.transform.position + new Vector3(-1.22f, 0, 0);
        evolvedText.transform.position -= new Vector3(-10, 0, 0);

        float secsToWait = 1.0f;
        foreach(GameObject c in chars) {
            StartCoroutine(Expand(c, secsToWait));
            secsToWait++;
        }
        StartCoroutine(Slide(evolvedText, secsToWait));
    }

    IEnumerator Expand(GameObject obj, float secsToWait) 
    {
        float elapsed = 0;
        yield return new WaitForSecondsRealtime(secsToWait);
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            // obj.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
            obj.transform.localScale = Vector3.Slerp(
                obj.transform.localScale, new Vector3(1.2f, 1.2f), Time.deltaTime);
            // print(string.Format("{0}, {1}", elapsed, updateSpeedSecs));
            yield return new WaitForEndOfFrame();
        }
        if (elapsed >= updateSpeedSecs - 0.1f) 
        {
            GetComponent<AudioSource>().Play();
            Instantiate(textHitParticle, obj.transform.position + particleOffset, Quaternion.identity);
        }
    }

    IEnumerator Slide(GameObject obj, float secsToWait)
    {
        float elapsed = 0;
        yield return new WaitForSecondsRealtime(secsToWait);

        while (elapsed < 2.0f) 
        {
            elapsed += Time.deltaTime;
            // obj.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
            obj.transform.position = Vector3.Slerp(obj.transform.position, evolvedTextFinalPos, Time.deltaTime);
            // print(string.Format("{0}, {1}", elapsed, updateSpeedSecs));
            yield return new WaitForEndOfFrame();
        }
        if (elapsed >= updateSpeedSecs - 0.1f) 
        {
            obj.GetComponent<AudioSource>().Play();
            Instantiate(textHitParticle, obj.transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }
}
