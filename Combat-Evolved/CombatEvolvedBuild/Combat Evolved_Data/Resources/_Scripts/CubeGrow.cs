using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrow : MonoBehaviour
{
    public float maxScale = 10.0f;
    public float speed = 1.0f;

    public List<GameObject> verticalLines;
    public List<GameObject> horizontalLines;

    private Vector3 originalPos;
    private float originalScale;
    private float endScale;
    private float updateSpeedSecs = 0.5f;

    void Start()
    {
        originalPos = transform.position;
        originalScale = transform.localScale.z;
        endScale = originalScale;
        GetComponent<Renderer>().material.color = Color.blue;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(ResizeOn());
        // transform.localScale = Mathf.MoveTowards(transform.localScale.z, endScale, Time.deltaTime * speed);
    }

    private IEnumerator ResizeOn() 
    {
        float elapsed = 0;
        while(elapsed < updateSpeedSecs) {
            elapsed += Time.deltaTime;
            // transform.localScale = new Vector3(transform.localScale.x,
            //     Mathf.MoveTowards(transform.localScale.y, maxScale, Time.deltaTime * (speed / 10)),
            //     transform.localScale.z
            //     );
            // transform.position = originalPos + transform.up * (transform.localScale.y / 2.0f + originalScale / 2.0f);
            transform.localScale = new Vector3(
                transform.localScale.x,
                Mathf.Lerp(transform.localScale.y, maxScale, elapsed / updateSpeedSecs),
                transform.localScale.z);

            foreach(GameObject horizontalLine in horizontalLines) 
            {
                horizontalLine.transform.localScale = new Vector3(
                    Mathf.Lerp(horizontalLine.transform.localScale.y, 0.55f, elapsed / updateSpeedSecs),
                    horizontalLine.transform.localScale.y,
                    horizontalLine.transform.localScale.z
                );
            }
            
            transform.position = originalPos - new Vector3(0, 1) + (transform.up) * (transform.localScale.y / 2.0f + originalScale / 2.0f);
            print(transform.position);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Rotate());
        yield return null;
    }

    private IEnumerator Rotate() 
    {
        float elapsed = 0;
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            transform.eulerAngles = new Vector3(
                Mathf.LerpAngle(transform.eulerAngles.x, -10, elapsed / updateSpeedSecs),
                Mathf.LerpAngle(transform.eulerAngles.y, 45, elapsed / updateSpeedSecs),
                Mathf.LerpAngle(transform.eulerAngles.z, -10, elapsed / updateSpeedSecs));
        
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    private IEnumerator GrowBar()
    {
        float elapsed = 0;
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Slerp(transform.localScale,
                new Vector3(transform.localScale.x,
                transform.localScale.y + 2,
                transform.localScale.z), elapsed / updateSpeedSecs
            );

            transform.position = Vector3.Slerp(transform.position,
                new Vector3(transform.position.x,
                transform.position.y + 2,
                transform.position.z), elapsed / updateSpeedSecs
            );
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
