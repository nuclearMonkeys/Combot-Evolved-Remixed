using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour
{
    public static CameraController instance = null;
    public List<Transform> targets;

    public float smoothTime = 0.5f;
    // set top margin to 2 so scoreboard wont cover player
    // top/right/down/right
    public Vector4 cameraMargin = new Vector4(2,0,0,0);
    // map zoom size to offset size
    // the more its zoomed in, the higher the margin
    public Vector2 zoomSizeBounds = new Vector2(3, 15);
    public Vector2 marginSizeBounds = new Vector2(4, 1);
    float sizeOffset;

    public Camera cam;

    public float shake_duration;
    public float shake_intensity;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        instance = this;
    }

    void LateUpdate()
    {
        for(int i = 0; i < targets.Count; i++) {
            if(targets[i] == null)
                return;
        }
        if(targets.Count > 0)
            Zoom();
    }

    // Zooms to encapsulate all players
    void Zoom()
    {
        float minX = targets.Min(t => t.transform.position.x) - cameraMargin.w;
        float maxX = targets.Max(t => t.transform.position.x) + cameraMargin.y;
        float minY = targets.Min(t => t.transform.position.y) - cameraMargin.z;
        float maxY = targets.Max(t => t.transform.position.y) + cameraMargin.x;
        float desiredWidth = maxX - minX;
        float desiredHeight = maxY - minY;
        float currentWidth = Screen.width;
        float currentHeight = Screen.height;
        float targetSize
            = desiredWidth > desiredHeight
            ? ((desiredWidth / currentWidth) * currentHeight) / 2.0f
            : ((desiredHeight / currentHeight) * currentWidth) / 2.0f
            ;
        // find the right offset margin
        sizeOffset = map(targetSize, zoomSizeBounds.x, zoomSizeBounds.y, marginSizeBounds.x, marginSizeBounds.y);
        targetSize += sizeOffset;
        this.cam.orthographicSize = Mathf.Lerp(this.cam.orthographicSize, targetSize, Time.deltaTime);

        Vector3 position = this.cam.transform.position;
        position.x = maxX * 0.5f + minX * 0.5f;
        position.y = maxY * 0.5f + minY * 0.5f;
        //this.cam.transform.position = position;
        this.cam.transform.position = Vector3.MoveTowards(this.cam.transform.position, position, 5 * Time.deltaTime);
    }

    Vector3 GetCenterPoint() {
        if(targets.Count == 1) 
            return targets[0].position;
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for(int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    float GetGreatestDistance() {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for(int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }
 
    public void ShakeCamera(){
        StartCoroutine(ShakeCameraEnumerator());
    }

    IEnumerator ShakeCameraEnumerator()
    {
        Quaternion originRotation = transform.rotation;

        float time = 0;
        while(time < shake_duration)
        {
            Vector2 shakeOffset = Random.insideUnitSphere;
            transform.position = (Vector3)transform.position + (Vector3)shakeOffset * shake_intensity;
            transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
                originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
                originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
                originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        transform.rotation = Quaternion.identity;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}