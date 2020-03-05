using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movingBlock : MonoBehaviour
{
    List<Vector3> positions = new List<Vector3>();
    int i = 0;
    public float speed = 1f;
    Transform box;
    Rigidbody2D rb;

    private float distance;

    public float lineWidth = 0.2f;
    public Color lineColor = Color.white;
    public Material m;
    float dotSize = 0.3f;

    private void Start()
    {
        box = transform.GetChild(0);
        rb = box.gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 0f;
        rb.gravityScale = 0f;
        foreach (Transform child in transform)
        {
            child.position = new Vector3(child.position.x, child.position.y, 0f);
            if (child.position != transform.GetChild(0).transform.position)
            {
                child.localScale = new Vector3(dotSize, dotSize, 1f);
                positions.Add(child.transform.position);
            }
            
        }

        for (int i = 0; i < positions.Count; i++)
        {
            LineRenderer ln = transform.GetChild(i).gameObject.AddComponent<LineRenderer>();
            ln.material = m;
            ln.widthMultiplier = lineWidth;
            ln.positionCount = 2;
            ln.endColor = lineColor;
            ln.startColor = lineColor;
            ln.SetPosition(0, positions[i]);
            if (i == positions.Count - 1)
            {
                ln.SetPosition(1, positions[0]);
            }
            else
            {
                ln.SetPosition(1, positions[i + 1]);
            }
        }
        box.position = positions[0];
        i = 1;
        rb.velocity = (positions[i] - box.position).normalized * speed;
        distance = Vector3.Distance(positions[i], box.position);
    }

    private void Update()
    {
        
        if (distance >= Vector3.Distance(positions[i], box.position))
        {
            rb.velocity = (positions[i] - box.position).normalized * speed;
        }
        if((positions[i] - box.position).magnitude <= .1f)
        {
            i++;
            if (i >= positions.Count)
            {
                i = 0;
            }
            rb.velocity = (positions[i] - box.position).normalized * speed;
        }
        distance = Vector3.Distance(positions[i], box.position);
        
        if (box.rotation.z != 0)
        {
            box.eulerAngles = new Vector3(box.rotation.x, box.rotation.y, 0f);
        }
    }

}
