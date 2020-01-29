using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour
{
    public static CameraController instance = null;
    public List<Transform> targets;

    private Vector3 velocity;

    public Vector3 offset;
    public float smoothTime = 0.5f;

    public float minZoom = 180f;
    public float maxZoom = 10f;
    public float zoomLimiter = 2.5f;

    public Camera cam;

    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay;
    public float shake_intensity;

    private void Start() {
        if(!instance)
            instance = this;
        else {
            Destroy(this.gameObject);
            return;
        }
    }
 
    void LateUpdate()
    {
        for(int i = 0; i < targets.Count; i++) {
            if(targets[i] == null)
                return;
        }
        if (shake_intensity > 0){
        transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
        transform.rotation = new Quaternion(
            originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
            originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
            originRotation.z + Random.Range (-shake_intensity,shake_intensity) * .2f,
            originRotation.w + Random.Range (-shake_intensity,shake_intensity) * .2f);
        shake_intensity -= shake_decay;
        }
        Zoom();
    }

/*
    void LateUpdate() {
        if(targets.Count == 0)
            return;

        Move();
        Zoom();
    }
*/
    void Move() {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
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

    void Zoom() {
        float minX = targets.Min(t => t.transform.position.x);
        float maxX = targets.Max(t => t.transform.position.x);
        float minY = targets.Min(t => t.transform.position.y);
        float maxY = targets.Max(t => t.transform.position.y);
        float desiredWidth = maxX - minX;
        float desiredHeight = maxY - minY;
        float currentWidth = Screen.width;
        float currentHeight = Screen.height;
        float targetSize
            = desiredWidth > desiredHeight
            ? ((desiredWidth / currentWidth) * currentHeight) / 2.0f
            : ((desiredHeight / currentHeight) * currentWidth) / 2.0f
            ;
        targetSize += 1.0f;
        this.cam.orthographicSize = Mathf.Lerp(this.cam.orthographicSize, targetSize, Time.deltaTime);
 
        Vector3 position = this.cam.transform.position;
        position.x = maxX * 0.5f + minX * 0.5f;
        position.y = maxY * 0.5f + minY * 0.5f;
        this.cam.transform.position = position;
    }

    void ZoomWinner() {
        float currentWidth = Screen.width;
        float currentHeight = Screen.height;

        //float 
    }
 
    public void ShakeCamera(){
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = 0.4f;
        shake_decay = 0.02f;
    }
}
