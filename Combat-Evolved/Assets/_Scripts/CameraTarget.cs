using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    public float margin;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;
        Bounds bounds = GetBounds();
        Vector3 centerPoint = bounds.center;
        Vector3 newPosition = centerPoint + offset;
        transform.position = newPosition;
        cam.rect = new Rect(0, 0, 1, 1);
        print("Min: " + bounds.min);
        print("Max: " + bounds.max);
    }

    Bounds GetBounds()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        if (targets.Count == 1)
        {
            return bounds;
        }
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        bounds.Expand(margin);
        return bounds;
    }

    public static void CalculateLimits(Camera aCam, Bounds aArea, out Rect aLimits, out float aMaxHeight)
    {
        var angle = aCam.fieldOfView * Mathf.Deg2Rad * 0.5f;
        Vector2 tan = Vector2.one * Mathf.Tan(angle);
        tan.x *= aCam.aspect;
        Vector3 dim = aArea.extents;
        Vector3 center = aArea.center - new Vector3(0, aArea.extents.y, 0);
        float maxDist = Mathf.Min(dim.x / tan.x, dim.z / tan.y);
        float dist = aCam.transform.position.y - center.y;
        float f = 1f - dist / maxDist;
        dim *= f;
        aMaxHeight = center.y + maxDist;
        aLimits = new Rect(center.x - dim.x, center.z - dim.z, dim.x * 2, dim.z * 2);
    }
}
